using Marketplace.Products.Application.Products.Models;
using Marketplace.Products.Domain.Products;

namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Поисковик товаров
/// </summary>
public interface IProductsSearcher
{
    /// <summary>
    /// Найти товары
    /// </summary>
    /// <param name="parameters">Параметры поиска</param>
    /// <param name="cancellationToken"></param>
    public Task<List<Product>> SearchAsync(SearchParameters parameters, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить примерное количество товаров по параметрам (считает до 1000)
    /// </summary>
    /// <param name="parameters">Параметры поиска</param>
    /// <param name="cancellationToken"></param>
    public Task<int> QuantifyAsync(SearchParameters parameters, CancellationToken cancellationToken = default);
}
