using Marketplace.Products.Application.Categories.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Queries.GetCategories;

/// <summary>
/// Запрос на получение всех категорий
/// </summary>
public record GetAllCategoriesQuery()
    : IRequest<List<CategoryWithSubcategoriesVM>>;
