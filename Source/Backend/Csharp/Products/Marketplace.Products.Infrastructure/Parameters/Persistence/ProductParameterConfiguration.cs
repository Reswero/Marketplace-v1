using Marketplace.Products.Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Products.Infrastructure.Parameters.Persistence;

internal class ProductParameterConfiguration : IEntityTypeConfiguration<ProductParameter>
{
    public void Configure(EntityTypeBuilder<ProductParameter> builder)
    {
        builder.Property(p => p.Value).HasMaxLength(50);

        builder.HasOne(p => p.CategoryParameter)
            .WithMany();
    }
}
