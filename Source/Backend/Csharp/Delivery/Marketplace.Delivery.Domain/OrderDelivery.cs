using Marketplace.Delivery.Domain.Exceptions;

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

    /// <summary>
    /// Добавить статус
    /// </summary>
    /// <param name="status">Статус</param>
    /// <exception cref="StatusAlreadySettedException"></exception>
    public void AddStatus(OrderDeliveryStatus status)
    {
        if (_statuses.Any(s => s.Type == status.Type))
            throw new StatusAlreadySettedException(status.Type);

        _statuses.Add(status);
    }
}
