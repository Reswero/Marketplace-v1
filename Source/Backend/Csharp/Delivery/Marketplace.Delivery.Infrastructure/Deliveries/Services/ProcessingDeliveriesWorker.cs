using Marketplace.Common.Transactions;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Domain;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Services;

/// <summary>
/// Обработчик доставок заказов находящихся в обработке
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class ProcessingDeliveriesWorker(IOrderDeliveriesRepository repository, IUnitOfWork unitOfWork,
    IOrdersServiceClient ordersClient)
{
    private readonly IOrderDeliveriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrdersServiceClient _ordersClient = ordersClient;

    /// <summary>
    /// Выполнить обработку
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var statusTypes = Enum.GetValues<OrderDeliveryStatusType>();
        Dictionary<OrderDeliveryStatusType, byte> typeIndexes = new(statusTypes.Length);
        for (byte i = 0; i < statusTypes.Length; i++)
        {
            typeIndexes[statusTypes[i]] = i;
        }

        var orders = await _repository.GetProcessingAsync(cancellationToken);
        List<NewDeliveryStatus> newStatuses = new(orders.Count);

        foreach (var order in orders)
        {
            var lastStatus = order.Statuses.MaxBy(s => s.Type);
            var currentStatusIndex = typeIndexes[lastStatus!.Type];
            var nextStatusType = statusTypes[currentStatusIndex + 1];

            OrderDeliveryStatus nextStatus = new(order, nextStatusType);
            order.AddStatus(nextStatus);

            await _repository.UpdateAsync(order, cancellationToken);

            NewDeliveryStatus newStatus = new(order.Id, nextStatus.Type, nextStatus.OccuredAt);
            newStatuses.Add(newStatus);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        // TODO: try catch, save to outbox
        foreach (var status in newStatuses)
        {
            await _ordersClient.ChangeDeliveryStatusAsync(status, cancellationToken);
        }
    }
}
