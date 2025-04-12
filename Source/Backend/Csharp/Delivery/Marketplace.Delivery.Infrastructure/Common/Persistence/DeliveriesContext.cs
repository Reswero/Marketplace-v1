using Marketplace.Delivery.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Marketplace.Delivery.Infrastructure.Common.Persistence;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options"></param>
internal class DeliveriesContext(DbContextOptions<DeliveriesContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Доставки заказов
    /// </summary>
    public DbSet<OrderDelivery> Deliveries { get; set; } = null!;
}
