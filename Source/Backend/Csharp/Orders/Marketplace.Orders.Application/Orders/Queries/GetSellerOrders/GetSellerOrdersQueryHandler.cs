using Marketplace.Common.Identity.User;
using Marketplace.Orders.Application.Common.Exceptions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetSellerOrders;

/// <summary>
/// Получить заказы продавца
/// </summary>
internal class GetSellerOrdersQueryHandler(IOrderProductsRepository repository, IUserIdentityProvider userIdentity)
    : IRequestHandler<GetSellerOrdersQuery, List<SellerOrderVM>>
{
    private readonly IOrderProductsRepository _repository = repository;
    private readonly IUserIdentityProvider _userIndentity = userIdentity;

    public async Task<List<SellerOrderVM>> Handle(GetSellerOrdersQuery request, CancellationToken cancellationToken)
    {
        if (_userIndentity.Id is null)
            throw new AccessDeniedException();

        Pagination pagination = new(request.Pagination.Offset, request.Pagination.Limit);
        var products = await _repository.GetSellerProductsAsync(_userIndentity.Id.Value, pagination, cancellationToken);

        return products.Select(p => new SellerOrderVM()
        {
            OrderId = p.OrderId,
            OrderProductId = p.Id,
            ProductId = p.ProductId,
            Quantity = p.Quantity,
            ProductPrice = p.ProductPrice,
            DiscountSize = p.DiscountSize,
            CreatedAt = p.Order!.CreatedAt,
            CurrentStatus = p.Order!.Statuses.OrderBy(s => s.Type).First().Type,
        }).ToList();
    }
}
