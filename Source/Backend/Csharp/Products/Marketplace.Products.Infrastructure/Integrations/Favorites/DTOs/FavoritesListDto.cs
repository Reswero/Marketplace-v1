namespace Marketplace.Products.Infrastructure.Integrations.Favorites.DTOs;

/// <summary>
/// Список избранного
/// </summary>
/// <param name="ProductIds">Идентификаторы товаров</param>
internal record FavoritesListDto(HashSet<int> ProductIds);
