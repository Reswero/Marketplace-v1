using Marketplace.Common.Outbox.Queue;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Domain;
using Microsoft.Extensions.Logging;

namespace Marketplace.Delivery.Infrastructure.Integrations.Orders;

/// <summary>
/// Клиент к сервису заказов с outbox'ом
/// </summary>
internal class OrdersServiceClientOutboxed(ILogger<OrdersServiceClientOutboxed> logger,
    IOrdersServiceClient client, IOutboxQueue<NewDeliveryStatus> outbox)
    : IOrdersServiceClientOutboxed
{
    private readonly ILogger<OrdersServiceClientOutboxed> _logger = logger;
    private readonly IOrdersServiceClient _client = client;
    private readonly IOutboxQueue<NewDeliveryStatus> _outbox = outbox;

    /// <inheritdoc/>
    public async Task ChangeDeliveryStatusesAsync(IReadOnlyList<NewDeliveryStatus> statuses, CancellationToken cancellationToken = default)
    {
        List<NewDeliveryStatus> notSendedStatuses = [];
        foreach (var status in statuses)
        {
            try
            {
                await _client.ChangeDeliveryStatusAsync(status, cancellationToken);
            }
            catch (Exception e)
            {
                notSendedStatuses.Add(status);
                _logger.LogError(e, "Произошла ошибка во время отправки статуса доставки. {Error}", e.Message);
            }
        }

        await _outbox.PushAsync([.. notSendedStatuses], cancellationToken);
    }
}
