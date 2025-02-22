namespace Marketplace.Orders.Application.Integrations.Products;

/// <summary>
/// Товар
/// </summary>
public record Product
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

