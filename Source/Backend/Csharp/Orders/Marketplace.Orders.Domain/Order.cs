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
        Status = OrderStatusType.Created;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; private set; }
    /// <summary>
    /// Идентификатор покупателя
    /// </summary>
    public int CustomerId { get; private set; }
    /// <summary>
    /// Статус
    /// </summary>
    public OrderStatusType Status { get; private set; }
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
    /// <summary>
    /// Дата доставки
    /// </summary>
    public DateTimeOffset? DeliveredAt { get; private set; } = null;
    /// <summary>
    /// Товары
    /// </summary>
    public IReadOnlyList<OrderProduct> Products => _products;

    /// <summary>
    /// Установить статус
    /// </summary>
    /// <param name="status">Статус</param>
    public void SetStatus(OrderStatusType status)
    {
        Status |= status;
    }

    /// <summary>
    /// Добавить товары
    /// </summary>
    /// <param name="products">Товары</param>
    public void AddProducts(params OrderProduct[] products)
    {
        if (products.Length == 0)
            return;

        _products.AddRange(products);
    }
}
