﻿using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models.Services.Payment;
using Marketplace.Payment.Grpc;

namespace Marketplace.Orders.Infrastructure.Integrations.Payment;

/// <summary>
/// Клиент к сервису платежей
/// </summary>
/// <param name="client"></param>
internal class PaymentServiceClient(PaymentService.PaymentServiceClient client)
    : IPaymentServiceClient
{
    private readonly PaymentService.PaymentServiceClient _client = client;

    /// <inheritdoc/>
    public async Task<string> CreatePaymentAsync(PaymentInfo info, CancellationToken cancellationToken = default)
    {
        PaymentRequest request = new()
        {
            OrderId = info.OrderId,
            PaybleAmount = info.TotalPrice
        };

        var response = await _client.CreatePaymentAsync(request, cancellationToken: cancellationToken);
        return response.Id;
    }
}
