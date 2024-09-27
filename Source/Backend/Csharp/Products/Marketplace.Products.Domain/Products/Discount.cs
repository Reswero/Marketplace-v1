namespace Marketplace.Products.Domain.Products;

/// <summary>
/// Скидка
/// </summary>
public class Discount
{
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// Величина (в процентах)
    /// </summary>
    public int Size { get; set; }
    /// <summary>
    /// Действительна до
    /// </summary>
    public DateOnly ValidUntil { get; set; }
}
