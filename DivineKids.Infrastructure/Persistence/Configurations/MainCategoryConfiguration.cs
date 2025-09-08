using DivineKids.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DivineKids.Infrastructure.Persistence.Configurations;

internal sealed class MainCategoryConfiguration : IEntityTypeConfiguration<MainCategory>
{
    public void Configure(EntityTypeBuilder<MainCategory> builder)
    {
        builder.ToTable("MainCategories");

        builder.HasKey(mc => mc.Id);

        builder.Property(mc => mc.Name).HasMaxLength(200).IsRequired();
        builder.Property(mc => mc.Description).HasMaxLength(2000);
        builder.Property(mc => mc.ImageUrl).HasMaxLength(2048);
        builder.Property(mc => mc.CreatedDate).IsRequired();
        builder.Property(mc => mc.ModifiedDate).IsRequired();
    }
}