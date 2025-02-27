using Marketplace.Common.Identity.User;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Получить заказы покупателя
/// </summary>
internal class GetCustomerOrdersQueryHandler(IUserIdentityProvider userIdentity, IOrdersRepository repository)
    : IRequestHandler<GetCustomerOrdersQuery, List<OrderVM>>
{
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IOrdersRepository _repository = repository;

    public async Task<List<OrderVM>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        Pagination pagination = new(request.Pagination.Offset, request.Pagination.Limit);

        var orders = await _repository.GetByCustomerAsync(_userIdentity.Id!.Value, pagination, cancellationToken);
        return orders.Select(o =>
        {
            var statuses = o.Statuses.Select(s => new OrderStatusVM(s.Type, s.OccuredAt))
                .ToList();
            var products = o.Products.Select(p => new OrderProductVM(p.ProductId, p.Quantity, p.ProductId, p.DiscountSize))
                .ToList();

            return new OrderVM
            {
                Id = o.Id,
                Statuses = statuses,
                Products = products
            };
        }).ToList();
    }
}
