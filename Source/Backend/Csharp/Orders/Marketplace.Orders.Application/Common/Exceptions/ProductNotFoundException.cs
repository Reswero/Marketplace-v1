namespace Marketplace.Orders.Application.Common.Exceptions;

public class ProductNotFoundException(int id)
    : Exception($"Товар с идентификатором {id} не найден")
{ }
