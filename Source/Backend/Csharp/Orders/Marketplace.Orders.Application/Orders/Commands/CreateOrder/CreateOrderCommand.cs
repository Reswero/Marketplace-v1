using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Application.Orders.DTOs;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Команда создания заказа
/// </summary>
/// <param name="CustomerId">Идентификатор покупателя</param>
/// <param name="Products">Товары</param>
public record CreateOrderCommand(int CustomerId, List<OrderProductDto> Products)
    : IRequest<CreateObjectResultVM>;
