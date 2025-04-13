using Marketplace.Delivery.Server;
using Marketplace.Orders.Application.Common.Interfaces;

namespace Marketplace.Orders.Infrastructure.Integrations.Delivery.Services;

/// <summary>
/// Клиент к сервису доставки
/// </summary>
/// <param name="client"></param>
internal class DeliveryServiceClient(DeliveryService.DeliveryServiceClient client) : IDeliveryServiceClient
{
    private readonly DeliveryService.DeliveryServiceClient _client = client;

    /// <inheritdoc/>
    public async Task CreateDeliveryAsync(int orderId, CancellationToken cancellationToken = default)
    {
        CreateDeliveryRequest request = new() { OrderId = orderId };
        await _client.CreateDeliveryAsync(request, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task CancelDeliveryAsync(int orderId, CancellationToken cancellationToken = default)
    {
        CancelDeliveryRequest request = new() { OrderId = orderId };
        await _client.CancelDeliveryAsync(request, cancellationToken: cancellationToken);
    }
}
