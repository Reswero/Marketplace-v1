﻿using Grpc.Core;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Domain;
using Marketplace.Orders.Server;

namespace Marketplace.Orders.Infrastructure.Orders.Services;

/// <summary>
/// gRPC-сервис заказов
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
public class OrdersGrpcService(IOrdersRepository repository, IUnitOfWork unitOfWork)
    : OrdersService.OrdersServiceBase
{
    private readonly IOrdersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Подтверждение оплаты заказа
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    public override async Task<ConfirmOrderPaymentResponse> ConfirmOrderPayment(
        ConfirmOrderPaymentRequest request, ServerCallContext context)
    {
        var order = await _repository.GetAsync(request.OrderId, context.CancellationToken);

        OrderStatus paidStatus = new(order, OrderStatusType.Paid);
        order.AddStatus(paidStatus);

        await _repository.UpdateAsync(order, context.CancellationToken);
        await _unitOfWork.CommitAsync(context.CancellationToken);

        return new ConfirmOrderPaymentResponse();
    }
}
