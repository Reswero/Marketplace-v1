namespace Marketplace.Delivery.Domain;

/// <summary>
/// Новый статус доставки
/// </summary>
public class NewDeliveryStatus
{
    private NewDeliveryStatus() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="type">Тип статуса</param>
    /// <param name="occuredAt">Произошло в</param>
    public NewDeliveryStatus(long orderId, OrderDeliveryStatusType type, DateTimeOffset occuredAt)
    {
        OrderId = orderId;
        Type = type;
        OccuredAt = occuredAt;
    }

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
