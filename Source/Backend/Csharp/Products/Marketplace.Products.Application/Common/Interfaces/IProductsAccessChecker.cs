namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Сервис проверки доступа к товару
/// </summary>
public interface IProductsAccessChecker
{
    /// <summary>
    /// Проверить доступ продавца к товару
    /// </summary>
    /// <param name="productId">Идентификатор товара</param>
    /// <param name="sellerId">Идентификатор продавца</param>
    /// <param name="cancellationToken"></param>
    /// <returns>true - доступ есть, false - доступа нет</returns>
    public Task<bool> CheckAccessAsync(int productId, int sellerId, CancellationToken cancellationToken = default);
}
