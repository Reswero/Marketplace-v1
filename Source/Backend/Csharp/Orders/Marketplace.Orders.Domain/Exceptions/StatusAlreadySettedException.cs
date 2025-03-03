namespace Marketplace.Orders.Domain.Exceptions;

/// <summary>
/// Статус уже установлен
/// </summary>
internal class StatusAlreadySettedException : Exception
{
    /// <summary>
    /// Статус уже установлен для заказа
    /// </summary>
    /// <param name="type">Тип</param>
    public StatusAlreadySettedException(OrderStatusType type)
        : base($"Статус {type} уже установлен для заказа")
    { }

    /// <summary>
    /// Статус уже установлен для товара
    /// </summary>
    /// <param name="type">Тип</param>
    public StatusAlreadySettedException(OrderProductStatusType type)
        : base($"Статус {type} уже установлен для товара")
    { }
}
