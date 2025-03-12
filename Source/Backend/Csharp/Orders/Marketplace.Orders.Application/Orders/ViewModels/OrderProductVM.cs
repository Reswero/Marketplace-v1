namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель товара заказа
/// </summary>
public record OrderProductVM()
{
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
    /// Статусы
    /// </summary>
    public List<OrderProductStatusVM> Statuses { get; set; } = [];
}
