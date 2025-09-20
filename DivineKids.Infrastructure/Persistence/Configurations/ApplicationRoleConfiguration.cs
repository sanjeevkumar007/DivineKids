using DivineKids.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DivineKids.Infrastructure.Persistence.Configurations;
public sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.Property(r => r.Name).HasMaxLength(256);
        builder.Property(r => r.NormalizedName).HasMaxLength(256);
        builder.Property(r => r.DisplayName).HasMaxLength(256);
        builder.Property(r => r.Description).HasMaxLength(1024);
    }
}
