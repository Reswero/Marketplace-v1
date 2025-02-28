using Marketplace.Orders.Application.Integrations.Products;

namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Клиент к сервису товаров
/// </summary>
public interface IProductsServiceClient
{
    /// <summary>
    /// Получить товары по идентификаторам
    /// </summary>
    /// <param name="ids">Идентификаторы</param>
    /// <param name="cancellationToken"></param>
    public Task<List<Product>> GetProductsByIdsAsync(int[] ids, CancellationToken cancellationToken = default);
}
