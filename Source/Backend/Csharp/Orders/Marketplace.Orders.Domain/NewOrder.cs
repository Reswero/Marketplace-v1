namespace Marketplace.Orders.Domain;

/// <summary>
/// Новый заказ
/// </summary>
public class NewOrder
{
    private NewOrder() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="order">Заказ</param>
    public NewOrder(Order order)
    {
        Id = order.Id;
        Order = order;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; private set; }
    /// <summary>
    /// Заказ
    /// </summary>
    public Order? Order { get; private set; }
}
