using Marketplace.Products.Application.Products.DTOs;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProductsByIdsInternal;

/// <summary>
/// Запрос получения товаров по идентификаторам
/// </summary>
/// <param name="Ids">Идентификаторы</param>
public record GetProductsByIdsInternalQuery(int[] Ids)
    : IRequest<List<ProductShortInfoDto>>;
