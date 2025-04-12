using Marketplace.Delivery.Application.Common.Exceptions;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Domain;
using Marketplace.Delivery.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Persistence;

/// <summary>
/// Репозиторий доставок заказов
/// </summary>
/// <param name="db">Контекст БД</param>
internal class OrderDeliveriesRepository(DeliveriesContext db)
    : IOrderDeliveriesRepository
{
    private readonly DeliveriesContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(OrderDelivery delivery, CancellationToken cancellationToken = default)
    {
        await _db.Deliveries.AddAsync(delivery, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<OrderDelivery> GetAsync(long orderId, CancellationToken cancellationToken = default)
    {
        return await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == orderId, cancellationToken)
            ?? throw new DeliveryNotFoundException(orderId);
    }

    /// <inheritdoc/>
    public async Task<List<OrderDelivery>> GetProcessingAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Deliveries.Include(d => d.Statuses)
            .Where(d => d.Statuses.All(s => s.Type < OrderDeliveryStatusType.Obtained))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(OrderDelivery delivery, CancellationToken cancellationToken = default)
    {
        _db.Deliveries.Update(delivery);
        return Task.CompletedTask;
    }
}
