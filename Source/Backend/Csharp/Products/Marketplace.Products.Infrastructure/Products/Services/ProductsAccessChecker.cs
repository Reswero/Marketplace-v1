using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Products.Infrastructure.Products.Services;

/// <summary>
/// Сервис проверки доступа к товару
/// </summary>
internal class ProductsAccessChecker(ProductsContext db) : IProductsAccessChecker
{
    private readonly ProductsContext _db = db;

    /// <inheritdoc/>
    public Task<bool> CheckAccessAsync(int productId, int sellerId, CancellationToken cancellationToken = default)
    {
        return _db.Products.Where(p => p.Id == productId && p.SellerId == sellerId)
            .AnyAsync(cancellationToken);
    }
}
