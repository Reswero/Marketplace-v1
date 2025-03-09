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

        if (request.ProductIds is null || request.ProductIds.Count == 0)
        {
            CancelProducts(order.Products, null);
            CancelOrder(order);
        }
        else
        {
            CancelProducts(order.Products, request.ProductIds);
            if (request.ProductIds.Count == order.Products.Count)
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

    private static void CancelProducts(IReadOnlyList<OrderProduct> products, HashSet<int>? productIds)
    {
        foreach (var product in products)
        {
            if (productIds is not null && productIds.Contains(product.ProductId) is false)
                throw new ProductNotFoundException(product.ProductId);

            OrderProductStatus cancelledStatus = new(product, OrderProductStatusType.Cancelled);
            product.AddStatus(cancelledStatus);
        }
    }
}
