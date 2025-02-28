namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель заказа
/// </summary>
public record OrderVM
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Статусы
    /// </summary>
    public List<OrderStatusVM> Statuses { get; set; } = [];
    /// <summary>
    /// Товары
    /// </summary>
    public List<OrderProductVM> Products { get; set; } = [];
}
