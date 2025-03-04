using Marketplace.Orders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Orders.Infrastructure.Orders.Persistence;

internal class OrderProductStatusConfiguration : IEntityTypeConfiguration<OrderProductStatus>
{
    public void Configure(EntityTypeBuilder<OrderProductStatus> builder)
    {
        builder.HasIndex(s => new { s.OrderProductId, s.Type });
    }
}
