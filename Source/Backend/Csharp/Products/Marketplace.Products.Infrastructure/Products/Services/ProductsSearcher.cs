using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Models;
using Marketplace.Products.Domain.Products;
using Marketplace.Products.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Products.Infrastructure.Products.Services;

/// <summary>
/// Поисковик товаров
/// </summary>
/// <param name="db">Контекст БД</param>
internal class ProductsSearcher(ProductsContext db) : IProductsSearcher
{
    private readonly ProductsContext _db = db;

    /// <inheritdoc/>
    public Task<List<Product>> SearchAsync(SearchParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = _db.Products.Include(p => p.Discounts)
            .Where(p => p.DeletedAt == null)
            .AsQueryable();

        query = ApplyParameters(query, parameters);
        return query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<int> QuantifyAsync(SearchParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = _db.Products.Include(p => p.Discounts)
            .Where(p => p.DeletedAt == null)
            .AsQueryable();

        query = ApplyParameters(query, parameters);
        return query.CountAsync(cancellationToken);
    }

    private static IQueryable<Product> ApplyParameters(IQueryable<Product> query, SearchParameters parameters)
    {
        query = ApplyFilters(query, parameters);
        query = ApplySort(query, parameters.SortType);
        query = ApplyPagination(query, parameters.Pagination);

        return query;
    }

    private static IQueryable<Product> ApplyFilters(IQueryable<Product> query, SearchParameters parameters)
    {
        if (parameters.Query is not null)
            query = query.Where(p => p.Name.ToUpper().Contains(parameters.Query.ToUpper()) ||
                p.Description.ToUpper().Contains(parameters.Query.ToUpper()));

        if (parameters.CategoryId is not null)
            query = query.Where(p => p.CategoryId == parameters.CategoryId);

        if (parameters.SubcategoryId is not null)
            query = query.Where(p => p.SubcategoryId == parameters.SubcategoryId);

        if (parameters.PriceFilter is not null)
        {
            query = query.Where(p => p.Price >= parameters.PriceFilter.From);
            if (parameters.PriceFilter.To is not null)
                query = query.Where(p => p.Price <= parameters.PriceFilter.To);
        }

        return query;
    }

    private static IQueryable<Product> ApplySort(IQueryable<Product> query, SortType type)
    {
        return type switch
        {
            SortType.PriceAscending => query.OrderBy(p => p.Price),
            SortType.PriceDescending => query.OrderByDescending(p => p.Price),
            SortType.BigDiscount => query.Where(p => p.Discounts.Count > 0)
                .OrderByDescending(p => p.Discounts.OrderBy(d => d.ValidUntil)
                    .FirstOrDefault()!.Size),
            SortType.Newness => query.OrderByDescending(p => p.CreatedAt),
            _ => throw new NotImplementedException(),
        };
    }

    private static IQueryable<Product> ApplyPagination(IQueryable<Product> query, Pagination pagination)
    {
        return query.Skip(pagination.Offset)
            .Take(pagination.Limit);
    }
}
