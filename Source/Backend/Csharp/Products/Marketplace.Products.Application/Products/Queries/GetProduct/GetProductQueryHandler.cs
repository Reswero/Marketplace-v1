using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Models;
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
internal class GetProductQueryHandler(ILogger<GetProductQueryHandler> logger, IUserIdentityProvider userIdentity,
    IProductsRepository repository, IUsersService usersService, IFavoritesService favoritesService)
    : IRequestHandler<GetProductQuery, ProductVM>
{
    private readonly ILogger<GetProductQueryHandler> _logger = logger;
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IProductsRepository _repository = repository;
    private readonly IUsersService _usersService = usersService;
    private readonly IFavoritesService _favoritesService = favoritesService;

    public async Task<ProductVM> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.Id, cancellationToken);
        var discount = product.Discounts!.MinBy(d => d.ValidUntil);

        var sellerTask = GetSellerAsync(product.SellerId, cancellationToken);
        var favoritesTask = CheckProductInFavorites(_userIdentity.Id, product.Id, cancellationToken);

        var seller = await sellerTask;
        var inFavorites = await favoritesTask;

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
            Status = product.DeletedAt is null ? ProductStatus.Available : ProductStatus.Deleted,
            InFavorites = inFavorites
        };
    }

    private async Task<Seller?> GetSellerAsync(int id, CancellationToken cancellationToken)
    {
        Seller? seller = null;
        try
        {
            seller = await _usersService.GetSellerInfoAsync(id, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время получения информации о продавце. {Error}", e.Message);
        }

        return seller;
    }

    private async Task<bool> CheckProductInFavorites(int? customerId, int productId, CancellationToken cancellationToken)
    {
        if (customerId is null)
            return false;

        try
        {
            var ids = await _favoritesService.CheckProductsInFavoritesAsync(customerId.Value, [productId], cancellationToken);
            if (ids.Contains(productId) is true)
                return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время проверки нахождения товара в избранном. {Error}", e.Message);
        }

        return false;
    }
}
