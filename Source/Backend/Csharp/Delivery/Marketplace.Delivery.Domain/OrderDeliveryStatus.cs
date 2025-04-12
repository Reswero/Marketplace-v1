namespace Marketplace.Delivery.Domain;

/// <summary>
/// Статус доставки заказа
/// </summary>
public class OrderDeliveryStatus
{
    private OrderDeliveryStatus() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="orderDelivery">Доставка заказа</param>
    /// <param name="type">Тип</param>
    public OrderDeliveryStatus(OrderDelivery orderDelivery, OrderDeliveryStatusType type)
    {
        OrderDeliveryId = orderDelivery.Id;
        Type = type;
    }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long OrderDeliveryId { get; private set; }
    /// <summary>
    /// Доставка заказа
    /// </summary>
    public OrderDelivery? OrderDelivery { get; private set; }
    /// <summary>
    /// Тип
    /// </summary>
    public OrderDeliveryStatusType Type { get; private set; }
    /// <summary>
    /// Произошло в
    /// </summary>
    public DateTimeOffset OccuredAt { get; private set; } = DateTimeOffset.UtcNow;
}
