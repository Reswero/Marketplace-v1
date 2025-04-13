using Grpc.Core;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Domain;
using Marketplace.Orders.Grpc;

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

    /// <summary>
    /// Изменение статуса доставки заказа
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <exception cref="NotSupportedException"></exception>
    public override async Task<ChangeDeliveryStatusResponse> ChangeDeliveryStatus(
        ChangeDeliveryStatusRequest request, ServerCallContext context)
    {
        var order = await _repository.GetAsync(request.OrderId, context.CancellationToken);

        var statusType = request.NewStatus switch
        {
            DeliveryStatus.Unknown => throw new NotSupportedException(),
            DeliveryStatus.Packed => OrderStatusType.Packed,
            DeliveryStatus.Shipped => OrderStatusType.Shipped,
            DeliveryStatus.InDelivery => OrderStatusType.InDelivery,
            DeliveryStatus.Obtained => OrderStatusType.Obtained,
            _ => throw new NotSupportedException()
        };
        var occuredAt = request.OccuredAt.ToDateTimeOffset();

        OrderStatus newStatus = new(order, statusType, occuredAt);
        order.AddStatus(newStatus);

        foreach (var product in order.Products)
        {
            var productStatusType = request.NewStatus switch
            {
                DeliveryStatus.Unknown => throw new NotSupportedException(),
                DeliveryStatus.Packed => OrderProductStatusType.Packed,
                DeliveryStatus.Shipped => OrderProductStatusType.Shipped,
                DeliveryStatus.InDelivery => OrderProductStatusType.InDelivery,
                DeliveryStatus.Obtained => OrderProductStatusType.Obtained,
                _ => throw new NotSupportedException()
            };

            OrderProductStatus newProductStatus = new(product, productStatusType, occuredAt);
            product.AddStatus(newProductStatus);
        }

        await _repository.UpdateAsync(order, context.CancellationToken);
        await _unitOfWork.CommitAsync(context.CancellationToken);

        return new ChangeDeliveryStatusResponse();
    }
}
