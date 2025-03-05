namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель краткой информации о заказе
/// </summary>
public record OrderShortInfoVM
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Итоговая стоимость
    /// </summary>
    public int TotalPrice { get; set; }
    /// <summary>
    /// Теущий статус
    /// </summary>
    public required OrderStatusVM CurrentStatus { get; set; }
    /// <summary>
    /// Товары
    /// </summary>
    public required List<OrderProductShortInfoVM> Products { get; set; }
}
