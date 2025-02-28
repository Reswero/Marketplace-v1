using Marketplace.Orders.Domain;

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
    /// Количество товаров
    /// </summary>
    public int ProductCount { get; set; }
    /// <summary>
    /// Итоговая стоимость
    /// </summary>
    public int TotalPrice { get; set; }
    /// <summary>
    /// Теущий статус
    /// </summary>
    public required CurrentStatusVM CurrentStatus { get; set; }
}
