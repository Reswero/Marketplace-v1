using Marketplace.Products.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Products.Infrastructure.Products.Persistence;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(1000);

        builder.HasOne(p => p.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(p => p.Subcategory)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Parameters)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);
        builder.HasMany(p => p.Discounts)
            .WithOne();
        builder.HasMany(p => p.Images)
            .WithOne();
    }
}
