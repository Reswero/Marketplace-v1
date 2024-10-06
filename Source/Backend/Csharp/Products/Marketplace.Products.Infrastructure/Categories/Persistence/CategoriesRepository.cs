using Marketplace.Products.Application.Common.Exceptions;
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
    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _db.Categories.AddAsync(category, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Category> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _db.Categories.Include(c => c.Parameters)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new ObjectNotFoundException(typeof(Category), id);

        return category;
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _db.Categories.Update(category);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        _db.Categories.Remove(category);
    }

    private readonly ProductsContext _db = db;
}
