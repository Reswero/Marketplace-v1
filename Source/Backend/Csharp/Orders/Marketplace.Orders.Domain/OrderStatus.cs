namespace Marketplace.Orders.Domain;

/// <summary>
/// Статус заказа
/// </summary>
public class OrderStatus
{
    private OrderStatus() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="type">Тип статуса</param>
    public OrderStatus(Order order, OrderStatusType type)
    {
        OrderId = order.Id;
        Order = order;
        Type = type;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="type">Тип статуса</param>
    /// <param name="occuredAt">Произошло в</param>
    public OrderStatus(Order order, OrderStatusType type, DateTimeOffset occuredAt)
    {
        OrderId = order.Id;
        Order = order;
        Type = type;
        OccuredAt = occuredAt;
    }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long OrderId { get; private set; }
    /// <summary>
    /// Заказ
    /// </summary>
    public Order? Order { get; private set; }
    /// <summary>
    /// Тип статуса
    /// </summary>
    public OrderStatusType Type { get; private set; }
    /// <summary>
    /// Произошло в
    /// </summary>
    public DateTimeOffset OccuredAt { get; private set; } = DateTimeOffset.UtcNow;
}
