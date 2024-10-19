using Marketplace.Products.Application.Products.Models;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.SearchProductsQuery;

/// <summary>
/// Запрос на поиск товаров
/// </summary>
/// <param name="Query">Текстовый запрос</param>
/// <param name="CategoryId">Идентификатор категории</param>
/// <param name="SubcategoryId">Идентификатор подкатегории</param>
/// <param name="PriceFilter">Фильтр цен</param>
/// <param name="Pagination">Пагинация</param>
/// <param name="SortType">Тип сортировки</param>
public record SearchProductsQuery(string? Query = null, int? CategoryId = null, int? SubcategoryId = null,
    PriceFilterVM? PriceFilter = null, PaginationVM? Pagination = null, SortType SortType = SortType.Newness)
    : IRequest<SearchProductsResultVM>;
