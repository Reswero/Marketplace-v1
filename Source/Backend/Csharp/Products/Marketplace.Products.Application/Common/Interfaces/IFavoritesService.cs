namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Взаимодействие с сервисом избранного
/// </summary>
public interface IFavoritesService
{
    /// <summary>
    /// Проверить находятся ли товары с избранном у покупателя
    /// </summary>
    /// <param name="customerId">Идентификатор покупателя</param>
    /// <param name="idsToCheck">Идентификаторы товаров для проверки</param>
    public Task<HashSet<int>> CheckProductsInFavorites(int customerId, List<int> idsToCheck);
}
