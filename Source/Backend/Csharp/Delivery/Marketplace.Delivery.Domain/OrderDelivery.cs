namespace Marketplace.Delivery.Domain;

/// <summary>
/// Доставка
/// </summary>
public class OrderDelivery
{
    private readonly List<OrderDeliveryStatus> _statuses = [];

    private OrderDelivery() { }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор</param>
    public OrderDelivery(long id)
    {
        Id = id;
    }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long Id { get; private set; }
    /// <summary>
    /// Статусы
    /// </summary>
    public IReadOnlyList<OrderDeliveryStatus> Statuses => _statuses;
}
