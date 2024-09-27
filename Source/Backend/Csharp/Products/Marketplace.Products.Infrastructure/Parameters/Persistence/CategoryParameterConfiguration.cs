using Marketplace.Products.Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Products.Infrastructure.Parameters.Persistence;

internal class CategoryParameterConfiguration : IEntityTypeConfiguration<CategoryParameter>
{
    public void Configure(EntityTypeBuilder<CategoryParameter> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(100);
    }
}
