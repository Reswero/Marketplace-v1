namespace Marketplace.Orders.Domain;

/// <summary>
/// Тип статуса для товара из заказа
/// </summary>
public enum OrderProductStatusType : byte
{
    /// <summary>
    /// Собирается
    /// </summary>
    Packing = 0,
    /// <summary>
    /// Собран
    /// </summary>
    Packed = 20,
    /// <summary>
    /// Отправлен
    /// </summary>
    Shipped = 40,
    /// <summary>
    /// В доставке
    /// </summary>
    InDelivery = 60,
    /// <summary>
    /// Получен
    /// </summary>
    Obtained = 80,
    /// <summary>
    /// Отменен
    /// </summary>
    Cancelled = 100
}
