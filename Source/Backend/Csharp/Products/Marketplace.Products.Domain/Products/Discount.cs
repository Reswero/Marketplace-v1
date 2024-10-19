namespace Marketplace.Products.Domain.Products;

/// <summary>
/// Скидка
/// </summary>
public class Discount
{
    private Discount() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="productId">Идентификатор товара</param>
    /// <param name="size">Размер</param>
    /// <param name="validUntil">Действительна до</param>
    public Discount(int productId, int size, DateOnly validUntil)
    {
        ProductId = productId;
        Size = size;
        ValidUntil = validUntil;
    }

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
