using Marketplace.Delivery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Persistence;

internal class OrderDeliveryStatusConfiguration : IEntityTypeConfiguration<OrderDeliveryStatus>
{
    public void Configure(EntityTypeBuilder<OrderDeliveryStatus> builder)
    {
        builder.HasKey(s => new { s.OrderDeliveryId, s.Type });
    }
}
