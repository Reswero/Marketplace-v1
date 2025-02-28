namespace Marketplace.Orders.Domain.Exceptions;

/// <summary>
/// Статус уже установлен для заказа
/// </summary>
/// <param name="type">Тип статуса</param>
internal class OrderStatusAlreadySettedException(OrderStatusType type)
    : Exception($"Статус {type} уже установлен для заказа")
{ }
