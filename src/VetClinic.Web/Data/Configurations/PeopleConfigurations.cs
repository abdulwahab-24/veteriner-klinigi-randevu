using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> b)
    {
        b.Property(u => u.FullName).HasMaxLength(100).IsRequired();
    }
}

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> b)
    {
        b.Property(o => o.FirstName).HasMaxLength(50).IsRequired();
        b.Property(o => o.LastName).HasMaxLength(50).IsRequired();
        b.Property(o => o.Phone).HasMaxLength(20).IsRequired();
        b.Property(o => o.Email).HasMaxLength(256);
        b.Property(o => o.Address).HasMaxLength(300);

        // Optional 1:1 with an Identity user (Member). One owner profile per user.
        b.HasOne(o => o.User).WithOne()
            .HasForeignKey<Owner>(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(o => o.UserId).IsUnique().HasFilter("[UserId] IS NOT NULL");
    }
}

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> b)
    {
        b.Property(p => p.Name).HasMaxLength(50).IsRequired();
        b.Property(p => p.Species).HasConversion<string>().HasMaxLength(20);
        b.Property(p => p.Sex).HasConversion<string>().HasMaxLength(10);
        b.Property(p => p.Breed).HasMaxLength(50);
        b.Property(p => p.WeightKg).HasPrecision(6, 2);
        b.Property(p => p.MicrochipNo).HasMaxLength(20);
        b.Property(p => p.Notes).HasMaxLength(1000);

        b.HasOne(p => p.Owner).WithMany(o => o.Pets)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(p => p.MicrochipNo).IsUnique().HasFilter("[MicrochipNo] IS NOT NULL");
        b.ToTable(t => t.HasCheckConstraint("CK_Pets_WeightKg", "[WeightKg] IS NULL OR [WeightKg] > 0"));
    }
}

public class VeterinarianConfiguration : IEntityTypeConfiguration<Veterinarian>
{
    public void Configure(EntityTypeBuilder<Veterinarian> b)
    {
        b.Property(v => v.FirstName).HasMaxLength(50).IsRequired();
        b.Property(v => v.LastName).HasMaxLength(50).IsRequired();
        b.Property(v => v.LicenseNo).HasMaxLength(30).IsRequired();
        b.Property(v => v.Phone).HasMaxLength(20);

        b.HasOne(v => v.User).WithOne()
            .HasForeignKey<Veterinarian>(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(v => v.UserId).IsUnique();
        b.HasIndex(v => v.LicenseNo).IsUnique();
    }
}
