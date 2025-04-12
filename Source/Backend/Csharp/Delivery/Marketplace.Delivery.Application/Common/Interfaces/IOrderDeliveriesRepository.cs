using Marketplace.Delivery.Domain;

namespace Marketplace.Delivery.Application.Common.Interfaces;

/// <summary>
/// Репозиторий доставок заказов
/// </summary>
public interface IOrderDeliveriesRepository
{
    /// <summary>
    /// Добавить доставку заказа
    /// </summary>
    /// <param name="delivery">Доставка заказа</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(OrderDelivery delivery, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить доставку заказа
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    public Task<OrderDelivery> GetAsync(long orderId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить доставки заказов находящиеся в обработке
    /// </summary>
    /// <param name="cancellationToken"></param>
    public Task<List<OrderDelivery>> GetProcessingAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить доставку заказа
    /// </summary>
    /// <param name="delivery">Доставка заказа</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(OrderDelivery delivery, CancellationToken cancellationToken = default);
}
