using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Infrastructure.Orders.Services;

/// <summary>
/// Обработчик новых заказов
/// </summary>
internal class NewOrdersWorker(INewOrdersRepository newOrdersRepository, IOrdersRepository ordersRepository,
    IUnitOfWork unitOfWork)
{
    private const int _batchSize = 10_000;

    private readonly TimeSpan _maxTimeForPayment = TimeSpan.FromMinutes(30);
    private readonly INewOrdersRepository _newOrdersRepository = newOrdersRepository;
    private readonly IOrdersRepository _ordersRepository = ordersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Запустить обработку
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var newOrders = await _newOrdersRepository.GetAsync(_batchSize, cancellationToken);

        foreach (var newOrder in newOrders)
        {
            var order = newOrder.Order!;
            if (order.Statuses.Any(s => s.Type > OrderStatusType.Pending))
            {
                await _newOrdersRepository.DeleteAsync(newOrder, cancellationToken);
                continue;
            }

            if (DateTimeOffset.UtcNow - order.CreatedAt > _maxTimeForPayment)
            {
                OrderStatus cancelledStatus = new(order, OrderStatusType.Cancelled);
                order.AddStatus(cancelledStatus);

                await _ordersRepository.UpdateAsync(order, cancellationToken);
                await _newOrdersRepository.DeleteAsync(newOrder, cancellationToken);
            } 
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
