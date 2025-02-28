using Marketplace.Products.Application.Products.Models;

namespace Marketplace.Products.Application.Products.DTOs;

/// <summary>
/// Краткая информация о товаре
/// </summary>
public record ProductShortInfoDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор продавца
    /// </summary>
    public int SellerId { get; set; }
    /// <summary>
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Размер скидки
    /// </summary>
    public int DiscountSize { get; set; }
    /// <summary>
    /// Состояние
    /// </summary>
    public ProductStatus Status { get; set; }
}
