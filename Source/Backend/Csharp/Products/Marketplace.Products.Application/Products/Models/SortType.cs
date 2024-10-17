namespace Marketplace.Products.Application.Products.Models;

/// <summary>
/// Тип сортировки
/// </summary>
public enum SortType
{
    /// <summary>
    /// Цена по возростанию
    /// </summary>
    PriceAscending = 0,
    /// <summary>
    /// Цена по убыванию
    /// </summary>
    PriceDescending = 1,
    /// <summary>
    ///  По величине скидки
    /// </summary>
    BigDiscount = 2,
    /// <summary>
    /// По новизне
    /// </summary>
    Newness = 3
}
