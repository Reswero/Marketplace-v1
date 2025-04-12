using Grpc.Core;
using Marketplace.Common.Transactions;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Domain;
using Marketplace.Delivery.Server;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Services;

public class DeliveryGrpcService(IOrderDeliveriesRepository repository, IUnitOfWork unitOfWork)
    : DeliveryService.DeliveryServiceBase
{
    private readonly IOrderDeliveriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<CreateDeliveryResponse> CreateDelivery(CreateDeliveryRequest request, ServerCallContext context)
    {
        OrderDelivery delivery = new(request.OrderId);

        OrderDeliveryStatus createdStatus = new(delivery, OrderDeliveryStatusType.Created);
        delivery.AddStatus(createdStatus);

        await _repository.AddAsync(delivery, context.CancellationToken);
        await _unitOfWork.CommitAsync(context.CancellationToken);

        return new CreateDeliveryResponse();
    }

    public override async Task<CancelDeliveryResponse> CancelDelivery(CancelDeliveryRequest request, ServerCallContext context)
    {
        var delivery = await _repository.GetAsync(request.OrderId, context.CancellationToken);

        OrderDeliveryStatus cancelledStatus = new(delivery, OrderDeliveryStatusType.Cancelled);
        delivery.AddStatus(cancelledStatus);

        await _repository.UpdateAsync(delivery, context.CancellationToken);
        await _unitOfWork.CommitAsync(context.CancellationToken);

        return new CancelDeliveryResponse();
    }
}
