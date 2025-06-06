﻿using Marketplace.Products.Domain.Products;

namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Репозиторий товаров
/// </summary>
public interface IProductsRepository
{
    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="product">Товар</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(Product product, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить товар
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    public Task<Product> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить товары
    /// </summary>
    /// <param name="ids">Идентификаторыs</param>
    /// <param name="cancellationToken"></param>
    public Task<List<Product>> GetAsync(int[] ids, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить товар
    /// </summary>
    /// <param name="product">Товар</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    /// <summary>
    /// Удалить товар
    /// </summary>
    /// <param name="product">Товар</param>
    /// <param name="cancellationToken"></param>
    public Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
}
