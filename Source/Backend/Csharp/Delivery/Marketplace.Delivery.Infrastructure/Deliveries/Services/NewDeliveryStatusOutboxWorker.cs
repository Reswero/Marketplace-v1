using Marketplace.Common.Outbox.Queue;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Domain;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Services;

/// <summary>
/// Обработчик новых статусов доставки из outbox'а
/// </summary>
/// <param name="outbox"></param>
/// <param name="ordersClient"></param>
internal class NewDeliveryStatusOutboxWorker(IOutboxQueue<NewDeliveryStatus> outbox, IOrdersServiceClient ordersClient)
{
    private readonly IOutboxQueue<NewDeliveryStatus> _outbox = outbox;
    private readonly IOrdersServiceClient _ordersClient = ordersClient;

    /// <summary>
    /// Выполнить обработку
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var statusCount = await _outbox.CountAsync(cancellationToken);
        var statuses = await _outbox.PeekAsync(statusCount, cancellationToken);

        foreach (var status in statuses)
        {
            await _ordersClient.ChangeDeliveryStatusAsync(status, cancellationToken);
            await _outbox.PopAsync(cancellationToken);
        }
    }
}
