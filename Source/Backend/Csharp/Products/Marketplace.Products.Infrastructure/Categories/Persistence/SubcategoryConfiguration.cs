using Marketplace.Products.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Products.Infrastructure.Categories.Persistence;

internal class SubcategoryConfiguration : IEntityTypeConfiguration<Subсategory>
{
    public void Configure(EntityTypeBuilder<Subсategory> builder)
    {
        builder.Property(s => s.Name).HasMaxLength(100);
    }
}
