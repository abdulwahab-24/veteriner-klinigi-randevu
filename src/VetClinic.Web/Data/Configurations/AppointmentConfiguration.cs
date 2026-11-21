using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    /// <summary>SQL filter matching statuses that block a time slot (Pending, Approved).</summary>
    public static readonly string BlockingStatusFilter =
        $"[Status] IN (N'{AppointmentStatus.Pending}', N'{AppointmentStatus.Approved}')";

    public void Configure(EntityTypeBuilder<Appointment> b)
    {
        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Appointments_Range", "[StartUtc] < [EndUtc]");
            t.HasCheckConstraint("CK_Appointments_DurationSnapshot",
                "[ServiceDurationMinutesSnapshot] > 0 AND DATEDIFF(minute, [StartUtc], [EndUtc]) = [ServiceDurationMinutesSnapshot]");
            t.HasCheckConstraint("CK_Appointments_PriceSnapshot", "[ServicePriceSnapshot] >= 0");
        });

        b.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(a => a.Notes).HasMaxLength(1000);
        b.Property(a => a.StatusReason).HasMaxLength(500);
        b.Property(a => a.ServiceNameSnapshot).HasMaxLength(100).IsRequired();
        b.Property(a => a.ServicePriceSnapshot).HasPrecision(10, 2);
        b.Property(a => a.RowVersion).IsRowVersion();

        b.HasOne(a => a.Pet).WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.Veterinarian).WithMany(v => v.Appointments)
            .HasForeignKey(a => a.VeterinarianId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.Service).WithMany()
            .HasForeignKey(a => a.ServiceId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.CreatedByUser).WithMany()
            .HasForeignKey(a => a.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.StatusChangedByUser).WithMany()
            .HasForeignKey(a => a.StatusChangedByUserId).OnDelete(DeleteBehavior.Restrict);

        // Overlap lookups (Start < newEnd AND End > newStart) for a veterinarian or a pet.
        b.HasIndex(a => new { a.VeterinarianId, a.StartUtc }, "IX_Appointments_Veterinarian_Start")
            .IncludeProperties(a => new { a.EndUtc, a.Status });
        b.HasIndex(a => new { a.PetId, a.StartUtc }, "IX_Appointments_Pet_Start")
            .IncludeProperties(a => new { a.EndUtc, a.Status });

        // Backstop for exact duplicate start times only. It does NOT prevent overlapping
        // intervals; the booking service's sp_getapplock strategy is responsible for that.
        b.HasIndex(a => new { a.VeterinarianId, a.StartUtc }, "UX_Appointments_Veterinarian_Start_Blocking")
            .IsUnique()
            .HasFilter(BlockingStatusFilter);
    }
}
