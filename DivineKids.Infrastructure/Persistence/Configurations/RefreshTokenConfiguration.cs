using DivineKids.Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DivineKids.Infrastructure.Persistence.Configurations;
internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);
        builder.Property(r => r.Token).HasMaxLength(200).IsRequired();
        builder.HasIndex(r => r.Token).IsUnique();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.Property(r => r.ExpiresAtUtc).IsRequired();
    }
}
