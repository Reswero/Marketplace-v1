using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Репозиторий товаров из заказов
/// </summary>
public interface IOrderProductsRepository
{
    /// <summary>
    /// Получить товары продавца
    /// </summary>
    /// <param name="sellerId">Идентификатор продавца</param>
    /// <param name="pagination">Пагинация</param>
    /// <param name="cancellationToken"></param>
    public Task<List<OrderProduct>> GetSellerProductsAsync(int sellerId, Pagination pagination, CancellationToken cancellationToken = default);
}
