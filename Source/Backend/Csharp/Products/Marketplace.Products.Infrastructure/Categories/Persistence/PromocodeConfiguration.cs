using Marketplace.Products.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Products.Infrastructure.Categories.Persistence;

internal class PromocodeConfiguration : IEntityTypeConfiguration<Promocode>
{
    public void Configure(EntityTypeBuilder<Promocode> builder)
    {
        builder.HasKey(p => new { p.CategoryId, p.ValidUntil });

        builder.Property(p => p.Code).HasMaxLength(20);
    }
}
