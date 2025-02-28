using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetCustomerOrder;

/// <summary>
/// Запрос на получение заказа покупателя
/// </summary>
/// <param name="OrderId">Идентификатор товара</param>
public record GetCustomerOrderQuery(long OrderId) : IRequest<OrderVM>;
