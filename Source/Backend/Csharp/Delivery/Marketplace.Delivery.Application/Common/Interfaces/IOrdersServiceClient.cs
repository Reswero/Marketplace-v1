using Marketplace.Delivery.Domain;

namespace Marketplace.Delivery.Application.Common.Interfaces;

/// <summary>
/// Клиент к сервису заказов
/// </summary>
public interface IOrdersServiceClient
{
    /// <summary>
    /// Изменить статус доставки заказа
    /// </summary>
    /// <param name="status">Статус</param>
    /// <param name="cancellationToken"></param>
    Task ChangeDeliveryStatusAsync(NewDeliveryStatus status, CancellationToken cancellationToken = default);
}
