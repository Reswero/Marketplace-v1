using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetSellerOrders;

/// <summary>
/// Запрос на получение заказов продавца
/// </summary>
/// <param name="Pagination">Пагинация</param>
public record GetSellerOrdersQuery(PaginationVM Pagination) : IRequest<List<SellerOrderVM>>;
