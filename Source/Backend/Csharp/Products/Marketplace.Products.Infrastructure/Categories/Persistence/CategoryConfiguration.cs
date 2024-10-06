using Marketplace.Products.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Products.Infrastructure.Categories.Persistence;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100);

        builder.HasMany(c => c.Parameters)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);
        builder.HasMany(c => c.Promocodes)
            .WithOne();
        builder.HasMany(c => c.Subсategories)
            .WithOne();
    }
}
