namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Клиент к сервису доставки
/// </summary>
public interface IDeliveryServiceClient
{
    /// <summary>
    /// Создать доставку заказа
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    public Task CreateDeliveryAsync(long orderId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Отменить доставку заказа
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    public Task CancelDeliveryAsync(long orderId, CancellationToken cancellationToken = default);
}