namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Заказ уже отменён
/// </summary>
public class OrderAlreadyCancelled()
    : Exception("Заказ уже отменён")
{ }
