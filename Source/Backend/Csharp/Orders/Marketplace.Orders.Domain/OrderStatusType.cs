namespace Marketplace.Orders.Domain;

/// <summary>
/// Тип статуса для заказа
/// </summary>
public enum OrderStatusType
{
    /// <summary>
    /// Создан
    /// </summary>
    Created = 0,
    /// <summary>
    /// Ожидает оплаты
    /// </summary>
    Pending = 20,
    /// <summary>
    /// Оплачен
    /// </summary>
    Paid = 40,
    /// <summary>
    /// Собран
    /// </summary>
    Packed = 60,
    /// <summary>
    /// Отправлено
    /// </summary>
    Shipped = 80,
    /// <summary>
    /// Доставлено
    /// </summary>
    InDelivery = 100,
    /// <summary>
    /// Получен
    /// </summary>
    Obtained = 120,
    /// <summary>
    /// Отменен
    /// </summary>
    Cancelled = 140
}
