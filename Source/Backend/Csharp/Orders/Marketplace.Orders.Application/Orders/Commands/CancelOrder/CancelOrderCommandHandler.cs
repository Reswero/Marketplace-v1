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

        OrderStatus cancelledStatus = new(order, OrderStatusType.Cancelled);
        order.AddStatus(cancelledStatus);

        foreach (var product in order.Products)
        {
            OrderProductStatus cancelledProductStatus = new(product, OrderProductStatusType.Cancelled);
            product.AddStatus(cancelledProductStatus);
        }

        await _repository.UpdateAsync(order, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
