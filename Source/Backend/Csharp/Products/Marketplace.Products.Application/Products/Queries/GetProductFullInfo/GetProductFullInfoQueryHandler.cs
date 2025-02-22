using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Models;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProductFullInfo;

internal class GetProductFullInfoQueryHandler(IProductsRepository repository)
    : IRequestHandler<GetProductFullInfoQuery, ProductFullInfoVM>
{
    private readonly IProductsRepository _repository = repository;

    public async Task<ProductFullInfoVM> Handle(GetProductFullInfoQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.Id, cancellationToken);

        return new ProductFullInfoVM()
        {
            Id = product.Id,
            Category = new(product.CategoryId, product.Category!.Name),
            Subcategory = new(product.SubcategoryId, product.Subcategory!.Name),
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Discounts = product.Discounts.Select(d => new DiscountVM(d.Size, d.ValidUntil)).ToList(),
            Parameters = product.Parameters.Select(p =>
                new ProductParameterVM(p.Id, p.CategoryParameterId, p.CategoryParameter!.Name, p.Value)).ToList(),
            Images = product.Images.Select(i => new ImageVM(i.Id, i.Order, $"{i.BucketName}/{i.Name}")).ToList(),
            Status = product.DeletedAt is null ? ProductStatus.Available : ProductStatus.Deleted,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            DeletedAt = product.DeletedAt
        };
    }
}
