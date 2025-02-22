using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.DTOs;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProductsByIdsInternal;

/// <summary>
/// Обработчик запроса на получение товаров по идентификаторам
/// </summary>
internal class GetProductsByIdsInternalQueryHandler(IProductsRepository repository)
    : IRequestHandler<GetProductsByIdsInternalQuery, List<ProductShortInfoDto>>
{
    private readonly IProductsRepository _repository = repository;

    public async Task<List<ProductShortInfoDto>> Handle(GetProductsByIdsInternalQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAsync(request.Ids, cancellationToken);
        return products.Select(p =>
        {
            var discount = p.Discounts.MinBy(d => d.ValidUntil);

            return new ProductShortInfoDto()
            {
                Id = p.Id,
                SellerId = p.SellerId,
                Price = p.Price,
                DiscountSize = discount is not null ? discount.Size : 0,
                Status = p.DeletedAt is null ? ProductStatus.Available : ProductStatus.Deleted
            };
        }).ToList();
    }
}
