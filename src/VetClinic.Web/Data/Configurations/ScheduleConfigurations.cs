using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data.Configurations;

public class ClinicWorkingHourConfiguration : IEntityTypeConfiguration<ClinicWorkingHour>
{
    public void Configure(EntityTypeBuilder<ClinicWorkingHour> b)
    {
        b.ToTable(t => t.HasCheckConstraint(
            "CK_ClinicWorkingHours_Range", "[IsClosed] = 1 OR [OpenTime] < [CloseTime]"));
        b.HasIndex(h => h.DayOfWeek).IsUnique();
    }
}

public class VeterinarianAvailabilityConfiguration : IEntityTypeConfiguration<VeterinarianAvailability>
{
    public void Configure(EntityTypeBuilder<VeterinarianAvailability> b)
    {
        b.ToTable(t => t.HasCheckConstraint(
            "CK_VeterinarianAvailabilities_Range", "[StartTime] < [EndTime]"));

        b.HasOne(a => a.Veterinarian).WithMany(v => v.Availabilities)
            .HasForeignKey(a => a.VeterinarianId).OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(a => new { a.VeterinarianId, a.DayOfWeek, a.StartTime });
    }
}

public class VeterinarianTimeOffConfiguration : IEntityTypeConfiguration<VeterinarianTimeOff>
{
    public void Configure(EntityTypeBuilder<VeterinarianTimeOff> b)
    {
        b.ToTable(t => t.HasCheckConstraint("CK_VeterinarianTimeOffs_Range", "[StartUtc] < [EndUtc]"));
        b.Property(t => t.Reason).HasMaxLength(200);

        b.HasOne(t => t.Veterinarian).WithMany(v => v.TimeOffs)
            .HasForeignKey(t => t.VeterinarianId).OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(t => new { t.VeterinarianId, t.StartUtc }).IncludeProperties(t => t.EndUtc);
    }
}
