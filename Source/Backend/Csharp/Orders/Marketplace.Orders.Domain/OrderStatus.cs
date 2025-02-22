namespace Marketplace.Orders.Domain;

/// <summary>
/// Статус заказа
/// </summary>
[Flags]
public enum OrderStatus
{
    /// <summary>
    /// Отсутствует
    /// </summary>
    None = 0,
    /// <summary>
    /// Создан
    /// </summary>
    Created = 1,
    /// <summary>
    /// Ожидает оплаты
    /// </summary>
    Pendign = 2,
    /// <summary>
    /// Оплачен
    /// </summary>
    Paid = 4,
    /// <summary>
    /// Упакован
    /// </summary>
    Packed = 8,
    /// <summary>
    /// Отправлено
    /// </summary>
    Shipped = 16,
    /// <summary>
    /// Доставлено
    /// </summary>
    InDelivery = 32,
    /// <summary>
    /// Получен
    /// </summary>
    Obtained = 64,
    /// <summary>
    /// Отменен
    /// </summary>
    Cancelled = 128
}
