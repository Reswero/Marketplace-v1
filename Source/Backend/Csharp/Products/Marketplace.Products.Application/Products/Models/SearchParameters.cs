namespace Marketplace.Products.Application.Products.Models;

public record SearchParameters(
    string? Query,
    int? CategoryId,
    int? SubcategoryId,
    PriceFilter? PriceFilter,
    Pagination Pagination,
    SortType SortType
);
