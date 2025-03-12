using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Репозиторий товаров из заказов
/// </summary>
public interface IOrderProductsRepository
{
    /// <summary>
    /// Получить товар
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    public Task<OrderProduct> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить товары продавца
    /// </summary>
    /// <param name="sellerId">Идентификатор продавца</param>
    /// <param name="pagination">Пагинация</param>
    /// <param name="cancellationToken"></param>
    public Task<List<OrderProduct>> GetBySellerAsync(int sellerId, Pagination pagination,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить товар
    /// </summary>
    /// <param name="product">Товар</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(OrderProduct product, CancellationToken cancellationToken = default);
}
