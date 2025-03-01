﻿using Marketplace.Common.Identity.User;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Application.Orders.ViewModels;
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
                ProductsIds = o.Products.Select(p => p.ProductId).ToArray(),
                ProductCount = o.Products.Select(p => p.Quantity).Sum(),
                TotalPrice =
                    (int) o.Products.Select(p => Math.Ceiling(p.ProductPrice * p.Quantity * (1 - p.DiscountSize / (double) 100))).Sum(),
                CurrentStatus = new(currentStatus.Type, currentStatus.OccuredAt)
            };
        }).ToList();
    }
}
