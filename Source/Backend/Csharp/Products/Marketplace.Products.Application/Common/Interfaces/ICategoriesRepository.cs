using Marketplace.Products.Domain.Categories;

namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Репозиторий категорий
/// </summary>
public interface ICategoriesRepository
{
    /// <summary>
    /// Добавить категорию
    /// </summary>
    /// <param name="category">Категория</param>
    /// <param name="cancellationToken"></param>
    public Task<int> AddAsync(Category category, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить категорию
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    public Task<Category> GetAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить категорию
    /// </summary>
    /// <param name="category">Категория</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
