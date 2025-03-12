using MediatR;

namespace Marketplace.Orders.Application.Orders.Commands.PackOrderProduct;

/// <summary>
/// Команда для установки товару статуса "Упакован"
/// </summary>
/// <param name="OrderProductId">Идентификатор товара из заказа</param>
public record PackOrderProductCommand(int OrderProductId) : IRequest;
