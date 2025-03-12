namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Невозможно отменить заказ
/// </summary>
public class ImpossibleToCancelOrderException()
    : Exception("Невозможно отменить заказ")
{ }
