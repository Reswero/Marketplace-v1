using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Products.Infrastructure.Common.Persistence;

/// <summary>
/// Репозиторий товаров
/// </summary>
/// <param name="db"></param>
internal class ProductsRepository(ProductsContext db) : IProductsRepository
{
    private readonly ProductsContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _db.Products.AddAsync(product, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Product> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Products.Include(p => p.Category)
            .Include(p => p.Subcategory)
            .Include(p => p.Discounts)
            .Include(p => p.Parameters)
                .ThenInclude(p => p.CategoryParameter)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new ObjectNotFoundException(typeof(Product), id);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _db.Products.Update(product);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        _db.Products.Remove(product);
        return Task.CompletedTask;
    }
}
