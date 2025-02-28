namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Заказ не найден
/// </summary>
/// <param name="id">Идентификатор</param>
public class OrderNotFoundException(long id)
    : Exception($"Заказ с идентификатором {id} не найден")
{ }
