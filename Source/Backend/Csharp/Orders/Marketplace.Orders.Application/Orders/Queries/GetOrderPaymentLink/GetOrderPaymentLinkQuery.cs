using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetOrderPaymentLink;

/// <summary>
/// Запрос на получение идентификатора платежа заказа
/// </summary>
/// <param name="OrderId">Идентификатор заказа</param>
public record GetOrderPaymentLinkQuery(long OrderId) : IRequest<PaymentVM>;
