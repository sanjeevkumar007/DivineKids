using DivineKids.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DivineKids.Infrastructure.Persistence.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.Property(c => c.ImageUrl).HasMaxLength(2048);
        builder.Property(c => c.CreatedDate).IsRequired();
        builder.Property(c => c.ModifiedDate).IsRequired();

        builder.HasOne<MainCategory>()
               .WithMany()
               .HasForeignKey(c => c.MainCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}