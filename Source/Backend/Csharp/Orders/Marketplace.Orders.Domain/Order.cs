using Marketplace.Orders.Domain.Exceptions;

namespace Marketplace.Orders.Domain;

/// <summary>
/// Заказ
/// </summary>
public class Order
{
    private readonly List<OrderProduct> _products = [];
    private readonly List<OrderStatus> _statuses = [];

    private Order() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="customerId">Идентификатор покупателя</param>
    public Order(int customerId)
    {
        CustomerId = customerId;
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
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    /// <summary>
    /// Статусы
    /// </summary>
    public IReadOnlyList<OrderStatus> Statuses => _statuses;
    /// <summary>
    /// Товары
    /// </summary>
    public IReadOnlyList<OrderProduct> Products => _products;

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

    /// <summary>
    /// Добавить статус
    /// </summary>
    /// <param name="status">Статус</param>
    public void AddStatus(OrderStatus status)
    {
        if (_statuses.Any(s => s.Type == status.Type))
            throw new OrderStatusAlreadySettedException(status.Type);

        _statuses.Add(status);
    }
}
