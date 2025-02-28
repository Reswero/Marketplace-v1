namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Невозможно оплатить заказ
/// </summary>
public class ImpossibleToPayOrderException()
    : Exception("Невозможно оплатить заказ")
{ }
