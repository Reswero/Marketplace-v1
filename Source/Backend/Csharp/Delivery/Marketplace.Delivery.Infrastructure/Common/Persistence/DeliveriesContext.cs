using Marketplace.Common.Transactions;
using Marketplace.Delivery.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Marketplace.Delivery.Infrastructure.Common.Persistence;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options"></param>
internal class DeliveriesContext(DbContextOptions<DeliveriesContext> options)
    : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc/>
    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Доставки заказов
    /// </summary>
    public DbSet<OrderDelivery> Deliveries { get; set; } = null!;
}
