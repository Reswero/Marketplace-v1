using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.CancelOrder;

/// <summary>
/// Команда на отмену заказа
/// </summary>
/// <param name="OrderId">Идентификатор заказа</param>
public record CancelOrderCommand(int OrderId) : IRequest;
