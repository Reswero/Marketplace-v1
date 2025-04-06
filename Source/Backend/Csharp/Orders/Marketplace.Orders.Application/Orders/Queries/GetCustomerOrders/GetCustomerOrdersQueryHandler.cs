using Marketplace.Common.Identity.User;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Application.Orders.ViewModels;
using Marketplace.Orders.Domain;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Получить заказы покупателя
/// </summary>
internal class GetCustomerOrdersQueryHandler(IUserIdentityProvider userIdentity, IOrdersRepository repository)
    : IRequestHandler<GetCustomerOrdersQuery, List<OrderShortInfoVM>>
{
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IOrdersRepository _repository = repository;

    public async Task<List<OrderShortInfoVM>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        Pagination pagination = new(request.Pagination.Offset, request.Pagination.Limit);

        var orders = await _repository.GetByCustomerAsync(_userIdentity.Id!.Value, pagination, cancellationToken);
        return orders.Select(o =>
        {
            var currentStatus = o.Statuses.OrderBy(s => s.Type).Last();

            return new OrderShortInfoVM
            {
                Id = o.Id,
                TotalPrice = o.GetTotalPrice(),
                CurrentStatus = new(currentStatus.Type, currentStatus.OccuredAt),
                Products = GetProductVMs(o.Products)
            };
        }).ToList();
    }

    private static List<OrderProductShortInfoVM> GetProductVMs(IReadOnlyList<OrderProduct> products)
    {
        List<OrderProductShortInfoVM> productVMs = new(products.Count);

        foreach (var product in products)
        {
            var currentStatus = product.Statuses.OrderBy(s => s.Type).Last();

            productVMs.Add(new OrderProductShortInfoVM
            {
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                CurrentStatusType = currentStatus.Type
            });
        }

        return productVMs;
    }
}
