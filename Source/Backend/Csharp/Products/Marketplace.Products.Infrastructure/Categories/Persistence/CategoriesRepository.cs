using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Products.Infrastructure.Categories.Persistence;

/// <summary>
/// Репозиторий категорий
/// </summary>
/// <param name="db"></param>
internal class CategoriesRepository(ProductsContext db) : ICategoriesRepository
{
    /// <inheritdoc/>
    public async Task<int> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _db.Categories.AddAsync(category, cancellationToken);
        return category.Id;
    }

    /// <inheritdoc/>
    public async Task<Category> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new Exception();

        return category;
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _db.Categories.Update(category);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category is null)
            return false;

        _db.Categories.Remove(category);
        return true;
    }

    private readonly ProductsContext _db = db;
}
