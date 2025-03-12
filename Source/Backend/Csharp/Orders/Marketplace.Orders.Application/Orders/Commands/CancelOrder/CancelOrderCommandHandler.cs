using Marketplace.Common.Identity.User;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Domain;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.CancelOrder;

/// <summary>
/// Отменить заказ
/// </summary>
internal class CancelOrderCommandHandler(IOrdersRepository repository, IUnitOfWork unitOfWork,
    IUserIdentityProvider userIdentity)
    : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrdersRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserIdentityProvider _userIdentity = userIdentity;

    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetAsync(request.OrderId, cancellationToken);

        if (_userIdentity.Id is null || order.CustomerId != _userIdentity.Id)
            throw new AccessDeniedException();

        if (order.Statuses.Any(s => s.Type >= OrderStatusType.Obtained))
            throw new ImpossibleToCancelOrderException();

        if (request.ProductIds is null || request.ProductIds.Length == 0)
        {
            CancelProducts(order.Products);
            CancelOrder(order);
        }
        else
        {
            CancelProducts(order.Products, request.ProductIds);
            var cancelledProductsCount = order.Products.Where(p => p.Statuses.Any(s => s.Type == OrderProductStatusType.Cancelled))
                .Count();

            if (cancelledProductsCount == order.Products.Count)
                CancelOrder(order);
        }

        await _repository.UpdateAsync(order, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private static void CancelOrder(Order order)
    {
        OrderStatus cancelledStatus = new(order, OrderStatusType.Cancelled);
        order.AddStatus(cancelledStatus);
    }

    private static void CancelProducts(IReadOnlyList<OrderProduct> products, int[]? productIdsToCancel = null)
    {
        var productsDict = products.ToDictionary(p => p.ProductId);

        if (productIdsToCancel is null || productIdsToCancel.Length == 0)
            productIdsToCancel = [.. productsDict.Keys];

        foreach (var id in productIdsToCancel)
        {
            if (productsDict.TryGetValue(id, out var product) is false)
                throw new ProductNotFoundException(id);

            OrderProductStatus cancelledStatus = new(product, OrderProductStatusType.Cancelled);
            product.AddStatus(cancelledStatus);
        }
    }
}
