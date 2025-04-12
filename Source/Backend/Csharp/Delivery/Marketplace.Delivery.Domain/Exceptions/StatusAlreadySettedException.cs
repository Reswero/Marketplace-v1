namespace Marketplace.Delivery.Domain.Exceptions;

/// <summary>
/// Статус уже установлен для доставки
/// </summary>
/// <param name="type">Тип статуса</param>
public class StatusAlreadySettedException(OrderDeliveryStatusType type)
    : Exception($"Статус {type} уже установлен для доставки")
{ }
