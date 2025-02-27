﻿using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;

namespace Marketplace.Orders.Application.Orders.Queries.GetOrderPaymentLink;

/// <summary>
/// Запрос на получение ссылки для оплаты заказа
/// </summary>
/// <param name="OrderId">Идентификатор заказа</param>
public record GetOrderPaymentLinkQuery(int OrderId) : IRequest<PaymentVM>;
