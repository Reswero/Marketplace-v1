namespace Marketplace.Delivery.Domain;

/// <summary>
/// Новый статус доставки
/// </summary>
public class NewDeliveryStatus
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long OrderId { get; private set; }
    /// <summary>
    /// Тип статуса
    /// </summary>
    public OrderDeliveryStatusType Type { get; private set; }
    /// <summary>
    /// Произошло в
    /// </summary>
    public DateTimeOffset OccuredAt { get; private set; }
}
