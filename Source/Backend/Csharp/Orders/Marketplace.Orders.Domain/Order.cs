namespace Marketplace.Orders.Domain;

/// <summary>
/// Заказ
/// </summary>
public class Order
{
    private readonly List<OrderProduct> _products = [];

    private Order() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="customerId">Идентификатор покупателя</param>
    public Order(int customerId)
    {
        CustomerId = customerId;
        Status = OrderStatus.Created;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Идентификатор покупателя
    /// </summary>
    public int CustomerId { get; private set; }
    /// <summary>
    /// Статус
    /// </summary>
    public OrderStatus Status { get; private set; }
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
    /// <summary>
    /// Дата доставки
    /// </summary>
    public DateTimeOffset? DeliveredAt { get; private set; } = null;
    
    public IReadOnlyList<OrderProduct> Products => _products;
}
