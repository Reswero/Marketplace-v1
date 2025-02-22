using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Models;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Marketplace.Products.Application.Products.Queries.GetProductsByIds;

internal class GetProductsByIdsQueryHandler(ILogger<GetProductsByIdsQueryHandler> logger, IProductsRepository repository,
    IUserIdentityProvider userIdentity, IFavoritesService favoritesService)
    : IRequestHandler<GetProductsByIdsQuery, List<ProductShortInfoVM>>
{
    private readonly ILogger<GetProductsByIdsQueryHandler> _logger = logger;
    private readonly IProductsRepository _repository = repository;
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IFavoritesService _favoritesService = favoritesService;

    public async Task<List<ProductShortInfoVM>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        var productsTask = _repository.GetAsync(request.Ids, cancellationToken);
        var favoritesTask = CheckProductsInFavoritesAsync(_userIdentity.Id, request.Ids, cancellationToken);

        var products = await productsTask;
        var favorites = await favoritesTask;

        return products.Select(p =>
        {
            var discount = p.Discounts.MinBy(d => d.ValidUntil);
            var image = p.Images.FirstOrDefault();

            return new ProductShortInfoVM()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Discount = discount is not null ? new(discount.Size, discount.ValidUntil) : null,
                Image = image is not null ? $"{image.BucketName}/{image.Name}" : null,
                Status = p.DeletedAt is null ? ProductStatus.Available : ProductStatus.Deleted,
                InFavorites = favorites.Contains(p.Id)
            };
        }).ToList();
    }

    private async Task<HashSet<int>> CheckProductsInFavoritesAsync(int? customerId, int[] productIds,
        CancellationToken cancellationToken)
    {
        if (customerId is null)
            return [];

        try
        {
            return await _favoritesService.CheckProductsInFavoritesAsync(customerId.Value, [.. productIds], cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время проверки нахождения товаров в избранном. {Error}", e.Message);
        }

        return [];
    }
}
