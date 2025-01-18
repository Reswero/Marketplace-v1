using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Models;
using Marketplace.Products.Application.Products.ViewModels;
using Marketplace.Products.Domain.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Marketplace.Products.Application.Products.Queries.SearchProductsQuery;

/// <summary>
/// Поиск товаров
/// </summary>
internal class SearchProductsQueryHandler(ILogger<SearchProductsQueryHandler> logger,
    IUserIdentityProvider userIdentity, IProductsSearcher searcher, IFavoritesService favoritesService)
    : IRequestHandler<SearchProductsQuery, SearchProductsResultVM>
{
    private readonly ILogger<SearchProductsQueryHandler> _logger = logger;
    private readonly IUserIdentityProvider _userIdentity = userIdentity;
    private readonly IProductsSearcher _searcher = searcher;
    private readonly IFavoritesService _favoritesService = favoritesService;

    public async Task<SearchProductsResultVM> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        PriceFilter priceFilter = request.PriceFilter is not null ?
            new(request.PriceFilter.From, request.PriceFilter.To) : new();
        Pagination pagination = request.Pagination is not null ?
            new(request.Pagination.Offset, request.Pagination.Limit) : new();

        SearchParameters parameters = new(request.Query, request.CategoryId, request.SubcategoryId,
            priceFilter, pagination, request.SortType);

        int count = await _searcher.QuantifyAsync(parameters, cancellationToken);
        List<Product> products = await _searcher.SearchAsync(parameters, cancellationToken);

        var productsInFavorites = await CheckProductsInFavoritesAsync(_userIdentity.Id, products.Select(p => p.Id).ToList(),
            cancellationToken);

        var productsVMs = products.Select(p =>
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
                InFavorites = productsInFavorites.Contains(p.Id)
            };
        }).ToList();

        return new(count, productsVMs);
    }

    private async Task<HashSet<int>> CheckProductsInFavoritesAsync(int? customerId, List<int> productIds,
        CancellationToken cancellationToken)
    {
        if (customerId is null)
            return [];

        try
        {
            return await _favoritesService.CheckProductsInFavoritesAsync(customerId.Value, productIds, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время проверки нахождения товаров в избранном. {Error}", e.Message);
        }

        return [];
    }
}
