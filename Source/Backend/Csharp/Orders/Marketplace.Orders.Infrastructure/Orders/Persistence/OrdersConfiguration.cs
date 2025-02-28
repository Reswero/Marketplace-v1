using Marketplace.Orders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Orders.Infrastructure.Orders.Persistence;

internal class OrdersConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => o.CustomerId);

        builder.HasMany(o => o.Products)
            .WithOne(p => p.Order);
        builder.HasMany(o => o.Statuses)
            .WithOne(p => p.Order);
    }
}
