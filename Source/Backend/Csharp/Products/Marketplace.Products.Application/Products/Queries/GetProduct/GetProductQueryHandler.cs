using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.ViewModels;
using Marketplace.Products.Application.Users.Models;
using Marketplace.Products.Application.Users.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Marketplace.Products.Application.Products.Queries.GetProduct;

/// <summary>
/// Получение товара
/// </summary>
/// <param name="repository"></param>
internal class GetProductQueryHandler(ILogger<GetProductQueryHandler> logger, IProductsRepository repository,
    IUsersService usersService)
    : IRequestHandler<GetProductQuery, ProductVM>
{
    private readonly ILogger<GetProductQueryHandler> _logger = logger;
    private readonly IProductsRepository _repository = repository;
    private readonly IUsersService _usersService = usersService;

    public async Task<ProductVM> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.Id, cancellationToken);
        var discount = product.Discounts!.MinBy(d => d.ValidUntil);

        Seller? seller = null;
        try
        {
            seller = await _usersService.GetSellerInfoAsync(product.SellerId, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время получения информации о продавце. {Error}", e.Message);
        }

        return new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Seller = seller is not null ? new SellerVM(seller.AccountId, seller.CompanyName) : null,
            Category = new(product.Category!.Id, product.Category!.Name),
            Subcategory = new(product.Subcategory!.Id, product.Subcategory!.Name),
            Discount = discount is not null ? new(discount.Size, discount.ValidUntil) : null,
            Parameters = product.Parameters.Select(p =>
                new ProductParameterVM(p.Id, p.CategoryParameterId, p.CategoryParameter!.Name, p.Value)).ToList(),
            Images = product.Images.Select(i => $"{i.BucketName}/{i.Name}").ToList(),
            Status = product.DeletedAt is null ? ProductStatus.Available : ProductStatus.Deleted
        };
    }
}
