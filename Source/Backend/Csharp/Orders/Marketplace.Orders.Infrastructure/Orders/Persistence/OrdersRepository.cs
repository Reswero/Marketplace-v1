using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Domain;
using Marketplace.Orders.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Orders.Infrastructure.Orders.Persistence;

/// <summary>
/// Репозиторий заказов
/// </summary>
internal class OrdersRepository(OrdersContext db) : IOrdersRepository
{
    private readonly OrdersContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _db.Orders.AddAsync(order, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Order> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Orders.Include(o => o.Products)
            .Where(o => o.Id == id).FirstOrDefaultAsync(cancellationToken)
                ?? throw new OrderNotFoundException(id);
    }

    /// <inheritdoc/>
    public async Task<List<Order>> GetByCustomerAsync(int customerId, Pagination pagination,
        CancellationToken cancellationToken = default)
    {
        return await _db.Orders.Include(o => o.Products)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(pagination.Offset)
            .Take(pagination.Limit)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Order>> GetBySellerAsync(int sellerId, Pagination pagination,
        CancellationToken cancellationToken = default)
    {
        return await _db.Orders.Include(o => o.Products)
            .Where(o => o.Products.Any(p => p.SellerId == sellerId))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _db.Orders.Update(order);
        return Task.CompletedTask;
    }
}
