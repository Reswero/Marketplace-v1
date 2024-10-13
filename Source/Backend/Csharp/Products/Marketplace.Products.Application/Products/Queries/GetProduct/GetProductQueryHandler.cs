using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProduct;

internal class GetProductQueryHandler(IProductsRepository repository)
    : IRequestHandler<GetProductQuery, ProductVM>
{
    private readonly IProductsRepository _repository = repository;

    public async Task<ProductVM> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.Id, cancellationToken);

        // TODO: Users Service
        //var seller = await _usersService.GetAsync(product.SellerId, cancellationToken);
        var discount = product.Discounts!.OrderBy(d => d.ValidUntil).FirstOrDefault();

        return new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Seller = new(0, "Sample Company"),
            Category = new(product.Category!.Id, product.Category!.Name),
            Subcategory = new(product.Subcategory!.Id, product.Subcategory!.Name),
            Discount = discount is not null ? new(discount.Size, discount.ValidUntil) : null,
            Parameters = product.Parameters.Select(p =>
                new ProductParameterVM(p.Id, p.CategoryParameterId, p.CategoryParameter!.Name, p.Value)).ToList(),
            Status = product.DeletedAt is null ? ProductStatus.Available : ProductStatus.Deleted
        };
    }
}
