namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Товар не доступен
/// </summary>
/// <param name="id">Идентификатор товара</param>
public class ProductNotAvailableException(int id)
    : Exception($"Товар с идентификатором {id} не доступен")
{ }
