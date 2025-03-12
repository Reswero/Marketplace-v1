using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.CancelOrder;

/// <summary>
/// Команда на отмену заказа
/// </summary>
/// <param name="OrderId">Идентификатор заказа</param>
/// <param name="ProductIds">Идентфикаторы товаров</param>
public record CancelOrderCommand(long OrderId, int[]? ProductIds) : IRequest;
