namespace Marketplace.Products.Infrastructure.Integrations.Favorites.DTOs;

/// <summary>
/// Модель проверки нахождения товаров в избранном
/// </summary>
/// <param name="ProductIdsToCheck">Идентификаторы для проверки</param>
internal record CheckFavoritesProductsDto(List<int> ProductIdsToCheck);
