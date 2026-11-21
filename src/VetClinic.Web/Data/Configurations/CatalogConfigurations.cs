using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> b)
    {
        b.Property(s => s.Name).HasMaxLength(100).IsRequired();
        b.HasIndex(s => s.Name).IsUnique();
    }
}

public class VeterinarianSpecialtyConfiguration : IEntityTypeConfiguration<VeterinarianSpecialty>
{
    public void Configure(EntityTypeBuilder<VeterinarianSpecialty> b)
    {
        b.HasKey(x => new { x.VeterinarianId, x.SpecialtyId });

        // Pure link table: removed together with either side.
        b.HasOne(x => x.Veterinarian).WithMany(v => v.Specialties)
            .HasForeignKey(x => x.VeterinarianId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.Specialty).WithMany(s => s.Veterinarians)
            .HasForeignKey(x => x.SpecialtyId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClinicServiceConfiguration : IEntityTypeConfiguration<ClinicService>
{
    public void Configure(EntityTypeBuilder<ClinicService> b)
    {
        b.ToTable("Services", t =>
        {
            t.HasCheckConstraint("CK_Services_DurationMinutes", "[DurationMinutes] BETWEEN 5 AND 480");
            t.HasCheckConstraint("CK_Services_Price", "[Price] >= 0");
        });

        b.Property(s => s.Name).HasMaxLength(100).IsRequired();
        b.Property(s => s.Description).HasMaxLength(500);
        b.Property(s => s.Price).HasPrecision(10, 2);
        b.HasIndex(s => s.Name).IsUnique();
    }
}

public class VeterinarianClinicServiceConfiguration : IEntityTypeConfiguration<VeterinarianClinicService>
{
    public void Configure(EntityTypeBuilder<VeterinarianClinicService> b)
    {
        b.ToTable("VeterinarianServices");
        b.HasKey(x => new { x.VeterinarianId, x.ServiceId });

        b.HasOne(x => x.Veterinarian).WithMany(v => v.Services)
            .HasForeignKey(x => x.VeterinarianId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.Service).WithMany(s => s.Veterinarians)
            .HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Cascade);
    }
}
