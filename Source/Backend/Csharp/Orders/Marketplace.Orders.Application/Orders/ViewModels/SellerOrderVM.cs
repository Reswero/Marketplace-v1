using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель заказа продавца
/// </summary>
public record SellerOrderVM
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long OrderId { get; set; }
    /// <summary>
    /// Идентификатор товара заказа
    /// </summary>
    public long OrderProductId { get; set; }
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Цена товара
    /// </summary>
    public int ProductPrice { get; set; }
    /// <summary>
    /// Размер скидки
    /// </summary>
    public int DiscountSize { get; set; }
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
    /// <summary>
    /// Текущий статус
    /// </summary>
    public OrderStatusType CurrentStatus { get; set; }
}
