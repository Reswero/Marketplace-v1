using Marketplace.Delivery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Persistence;

internal class OrderDeliveryConfiguration : IEntityTypeConfiguration<OrderDelivery>
{
    public void Configure(EntityTypeBuilder<OrderDelivery> builder)
    {
        builder.HasMany(d => d.Statuses)
            .WithOne(s => s.OrderDelivery)
            .HasForeignKey(s => s.OrderDeliveryId);
    }
}
