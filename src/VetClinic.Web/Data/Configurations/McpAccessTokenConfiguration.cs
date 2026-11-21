using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data.Configurations;

public class McpAccessTokenConfiguration : IEntityTypeConfiguration<McpAccessToken>
{
    public void Configure(EntityTypeBuilder<McpAccessToken> b)
    {
        b.Property(t => t.Name).HasMaxLength(100).IsRequired();
        b.Property(t => t.TokenPrefix).HasMaxLength(12).IsRequired();
        b.Property(t => t.TokenHash).HasMaxLength(32).IsFixedLength().IsRequired(); // binary(32)
        b.Property(t => t.Scopes).HasMaxLength(200).IsRequired();

        // Deleting a user removes their credentials as well.
        b.HasOne(t => t.User).WithMany()
            .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(t => t.TokenHash).IsUnique();
        b.HasIndex(t => t.UserId);
    }
}
