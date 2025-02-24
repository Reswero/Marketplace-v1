namespace Marketplace.Orders.Domain;

/// <summary>
/// Товар заказа
/// </summary>
public class OrderProduct
{
    private OrderProduct() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="productId">Идентификатор товара</param>
    /// <param name="sellerId">Идентификатор продавца</param>
    /// <param name="quantity">Количество</param>
    /// <param name="productPrice">Цена товара</param>
    /// <param name="discountSize">Размер скидки</param>
    public OrderProduct(Order order, int productId, int sellerId, int quantity,
        int productPrice, int discountSize)
    {
        OrderId = order.Id;
        Order = order;
        ProductId = productId;
        SellerId = sellerId;
        Quantity = quantity;
        ProductPrice = productPrice;
        DiscountSize = discountSize;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; private set; }
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long OrderId { get; private set; }
    /// <summary>
    /// Заказ
    /// </summary>
    public Order? Order { get; private set; } = null;
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public int ProductId { get; private set; }
    /// <summary>
    /// Идентификатор продавца
    /// </summary>
    public int SellerId { get; private set; }
    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; private set; }
    /// <summary>
    /// Цена товара
    /// </summary>
    public int ProductPrice { get; private set; }
    /// <summary>
    /// Размер скидки
    /// </summary>
    public int DiscountSize { get; private set; }
}
