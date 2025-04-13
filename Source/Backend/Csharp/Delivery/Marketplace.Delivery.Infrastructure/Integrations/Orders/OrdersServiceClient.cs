using Google.Protobuf.WellKnownTypes;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Domain;
using Marketplace.Orders.Grpc;

namespace Marketplace.Delivery.Infrastructure.Integrations.Orders;

/// <summary>
/// Клиент к сервису заказов
/// </summary>
/// <param name="client"></param>
internal class OrdersServiceClient(OrdersService.OrdersServiceClient client)
    : IOrdersServiceClient
{
    private readonly OrdersService.OrdersServiceClient _client = client;

    /// <inheritdoc/>
    public async Task ChangeDeliveryStatusAsync(NewDeliveryStatus status, CancellationToken cancellationToken = default)
    {
        var statusType = status.Type switch
        {
            OrderDeliveryStatusType.Packed => DeliveryStatus.Packed,
            OrderDeliveryStatusType.Shipped => DeliveryStatus.Shipped,
            OrderDeliveryStatusType.InDelivery => DeliveryStatus.InDelivery,
            OrderDeliveryStatusType.Obtained => DeliveryStatus.Obtained,
            _ => throw new NotSupportedException(),
        };

        ChangeDeliveryStatusRequest request = new()
        {
            OrderId = status.OrderId,
            NewStatus = statusType,
            OccuredAt = Timestamp.FromDateTimeOffset(status.OccuredAt)
        };

        // TODO: try catch, save to outbox
        await _client.ChangeDeliveryStatusAsync(request, cancellationToken: cancellationToken);
    }
}
