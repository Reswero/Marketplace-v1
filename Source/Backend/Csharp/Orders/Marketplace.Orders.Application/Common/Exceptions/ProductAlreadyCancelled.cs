namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Товар уже отменён
/// </summary>
public class ProductAlreadyCancelled()
    : Exception("Товар уже отменён")
{ }
