using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProductsByIds;

/// <summary>
/// Запрос получения товаров по идентификаторам
/// </summary>
/// <param name="Ids">Идентификаторы</param>
public record GetProductsByIdsQuery(int[] Ids) : IRequest<List<ProductShortInfoVM>>;
