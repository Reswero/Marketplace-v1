using Marketplace.Common.Identity.User;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Orders.ViewModels;
using Marketplace.Orders.Domain;
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
            Id = order.Id,
            Products = GetProductVMs(order.Products),
            Statuses = order.Statuses.Select(s => new OrderStatusVM(s.Type, s.OccuredAt)).ToList()
        };
    }

    private static List<OrderProductVM> GetProductVMs(IReadOnlyList<OrderProduct> products)
    {
        List<OrderProductVM> productVMs = new(products.Count);

        foreach (var product in products)
        {
            List<OrderProductStatusVM> statuses = new(product.Statuses.Count);
            foreach (var status in product.Statuses)
            {
                statuses.Add(new(status.Type, status.OccuredAt));
            }

            productVMs.Add(new OrderProductVM
            {
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                ProductPrice = product.ProductPrice,
                DiscountSize = product.DiscountSize,
                Statuses = statuses
            });
        }

        return productVMs;
    }
}
