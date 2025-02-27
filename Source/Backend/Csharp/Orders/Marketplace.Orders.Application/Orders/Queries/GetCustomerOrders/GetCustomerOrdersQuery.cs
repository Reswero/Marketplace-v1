using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Запрос на получение заказов покупателя 
/// </summary>
/// <param name="Pagination">Пагинация</param>
public record GetCustomerOrdersQuery(PaginationVM Pagination) : IRequest<List<OrderVM>>;
