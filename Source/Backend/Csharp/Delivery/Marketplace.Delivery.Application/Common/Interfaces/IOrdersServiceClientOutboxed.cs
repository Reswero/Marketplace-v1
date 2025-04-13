using Marketplace.Delivery.Domain;

namespace Marketplace.Delivery.Application.Common.Interfaces;

/// <summary>
/// Клиент к сервису заказов с outbox'ом
/// </summary>
public interface IOrdersServiceClientOutboxed
{
    /// <summary>
    /// Изменить статусы доставок заказов
    /// </summary>
    /// <param name="statuses">Статусы</param>
    /// <param name="cancellationToken"></param>
    Task ChangeDeliveryStatusesAsync(IReadOnlyList<NewDeliveryStatus> statuses, CancellationToken cancellationToken = default);
}
