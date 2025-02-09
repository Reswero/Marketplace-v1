using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.CheckProductsExistence;

/// <summary>
/// Запрос проверки существования товаров
/// </summary>
/// <param name="Ids">Идентификаторы</param>
public record CheckProductsExistenceQuery(int[] Ids) : IRequest<ExistingProductsVM>;
