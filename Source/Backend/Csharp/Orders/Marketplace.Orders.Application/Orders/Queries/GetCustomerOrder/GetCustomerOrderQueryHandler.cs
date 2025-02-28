using Marketplace.Common.Identity.User;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetCustomerOrder;

/// <summary>
/// Получение заказа покупателя
/// </summary>
internal class GetCustomerOrderQueryHandler(IOrdersRepository repository, IUserIdentityProvider userIdentity)
    : IRequestHandler<GetCustomerOrderQuery, OrderVM>
{
    private readonly IOrdersRepository _repository = repository;
    private readonly IUserIdentityProvider _userIdentity = userIdentity;

    public async Task<OrderVM> Handle(GetCustomerOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetAsync(request.OrderId, cancellationToken);

        if (_userIdentity.Id is null || order.CustomerId != _userIdentity.Id.Value)
            throw new AccessDeniedException();

        return new()
        {
            Id = order.CustomerId,
            Products =
                order.Products.Select(p => new OrderProductVM(p.ProductId, p.Quantity, p.ProductPrice, p.DiscountSize)).ToList(),
            Statuses = order.Statuses.Select(s => new OrderStatusVM(s.Type, s.OccuredAt)).ToList()
        };
    }
}
