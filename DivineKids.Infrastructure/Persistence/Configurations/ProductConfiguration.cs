using DivineKids.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DivineKids.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(p => p.Description)
               .HasMaxLength(2000);

        builder.Property(p => p.Price)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(p => p.ImageUrl)
               .HasMaxLength(2048);

        builder.Property(p => p.CreatedDate).IsRequired();
        builder.Property(p => p.ModifiedDate).IsRequired();

        builder.Property(p => p.RequiresShipping)
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(p => p.WeightKg);
        builder.Property(p => p.LengthCm);
        builder.Property(p => p.WidthCm);
        builder.Property(p => p.HeightCm);


        builder.HasOne<Category>()
               .WithMany()
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}