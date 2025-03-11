using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Domain;
using Marketplace.Orders.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Orders.Infrastructure.Orders.Persistence;

/// <summary>
/// Репозиторий товаров из заказов
/// </summary>
/// <param name="db">Контекст БД</param>
internal class OrderProductsRepository(OrdersContext db)
    : IOrderProductsRepository
{
    private readonly OrdersContext _db = db;

    /// <inheritdoc/>
    public async Task<List<OrderProduct>> GetBySellerAsync(int sellerId, Pagination pagination,
        CancellationToken cancellationToken = default)
    {
        return await _db.OrderProducts.Include(p => p.Order)
            .Include(p => p.Statuses)
            .Where(p => p.SellerId == sellerId)
            .OrderByDescending(p => p.Order!.CreatedAt)
            .Skip(pagination.Offset)
            .Take(pagination.Limit)
            .ToListAsync(cancellationToken);
    }
}
