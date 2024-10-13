using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Queries.GetProduct;

/// <summary>
/// Запрос получения товара
/// </summary>
/// <param name="Id">Идентификатор</param>
public record GetProductQuery(int Id) : IRequest<ProductVM>;
