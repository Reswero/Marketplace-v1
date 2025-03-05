using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель краткой информации о товаре из заказа
/// </summary>
public class OrderProductShortInfoVM
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
    /// Тип текущего статуса
    /// </summary>
    public required OrderProductStatusType CurrentStatusType { get; set; }
}
