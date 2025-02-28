using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Domain;
using Marketplace.Orders.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Orders.Infrastructure.Orders.Persistence;

/// <summary>
/// Репозиторий новых заказов
/// </summary>
/// <param name="db">Контекст БД</param>
internal class NewOrdersRepository(OrdersContext db) : INewOrdersRepository
{
    private readonly OrdersContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(NewOrder newOrder, CancellationToken cancellationToken = default)
    {
        await _db.NewOrders.AddAsync(newOrder, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<NewOrder>> GetAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _db.NewOrders.Include(o => o.Order)
            .ThenInclude(o => o!.Statuses)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task DeleteAsync(NewOrder order, CancellationToken cancellationToken = default)
    {
        _db.NewOrders.Remove(order);
        return Task.CompletedTask;
    }
}
