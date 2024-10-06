using Marketplace.Products.Application.Categories.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Queries.GetCategory;

/// <summary>
/// Запрос получения категории
/// </summary>
/// <param name="Id">Идентификатор</param>
public record GetCategoryQuery(int Id) : IRequest<CategoryVM>;
