using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data.Configurations;

public class ExaminationRecordConfiguration : IEntityTypeConfiguration<ExaminationRecord>
{
    public void Configure(EntityTypeBuilder<ExaminationRecord> b)
    {
        b.Property(e => e.Complaint).HasMaxLength(1000);
        b.Property(e => e.Diagnosis).HasMaxLength(2000);
        b.Property(e => e.Treatment).HasMaxLength(2000);
        b.Property(e => e.Prescription).HasMaxLength(2000);
        b.Property(e => e.Notes).HasMaxLength(2000);
        b.Property(e => e.WeightKg).HasPrecision(6, 2);
        b.Property(e => e.Temperature).HasPrecision(4, 1);

        // Optional 1:1 — at most one examination record per appointment.
        b.HasOne(e => e.Appointment).WithOne(a => a.ExaminationRecord)
            .HasForeignKey<ExaminationRecord>(e => e.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(e => e.AppointmentId).IsUnique().HasFilter("[AppointmentId] IS NOT NULL");

        b.HasOne(e => e.Pet).WithMany(p => p.ExaminationRecords)
            .HasForeignKey(e => e.PetId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Veterinarian).WithMany()
            .HasForeignKey(e => e.VeterinarianId).OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(e => new { e.PetId, e.ExaminationDateUtc });
    }
}

public class VaccinationConfiguration : IEntityTypeConfiguration<Vaccination>
{
    public void Configure(EntityTypeBuilder<Vaccination> b)
    {
        b.Property(v => v.VaccineName).HasMaxLength(100).IsRequired();
        b.Property(v => v.BatchNumber).HasMaxLength(50);
        b.Property(v => v.Notes).HasMaxLength(1000);

        b.HasOne(v => v.Pet).WithMany(p => p.Vaccinations)
            .HasForeignKey(v => v.PetId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(v => v.Veterinarian).WithMany()
            .HasForeignKey(v => v.VeterinarianId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(v => v.ExaminationRecord).WithMany(e => e.Vaccinations)
            .HasForeignKey(v => v.ExaminationRecordId).OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(v => new { v.PetId, v.AdministeredDateUtc });
        b.HasIndex(v => v.NextDueDate);
    }
}
