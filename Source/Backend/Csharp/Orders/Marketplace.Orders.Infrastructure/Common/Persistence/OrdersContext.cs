using Marketplace.Common.Transactions;
using Marketplace.Orders.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Marketplace.Orders.Infrastructure.Common.Persistence;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options"></param>
internal class OrdersContext(DbContextOptions<OrdersContext> options)
    : DbContext(options), IUnitOfWork
{
    /// <summary>
    /// Заказы
    /// </summary>
    public DbSet<Order> Orders { get; set; } = null!;
    /// <summary>
    /// Товары заказов
    /// </summary>
    public DbSet<OrderProduct> OrderProducts { get; set; } = null!;
    /// <summary>
    /// Новые заказы
    /// </summary>
    public DbSet<NewOrder> NewOrders { get; set; } = null!;

    /// <inheritdoc/>
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
