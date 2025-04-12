namespace Marketplace.Delivery.Domain;

/// <summary>
/// Тип статуса доставки заказа
/// </summary>
public enum OrderDeliveryStatusType
{
    /// <summary>
    /// Создан
    /// </summary>
    Created = 0,
    /// <summary>
    /// Собран
    /// </summary>
    Packed = 20,
    /// <summary>
    /// Отправлено
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
