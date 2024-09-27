using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Domain.Products;

/// <summary>
/// Товар
/// </summary>
public class Product
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
    /// Идентификатор категории
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Категория
    /// </summary>
    public Category? Category { get; set; }
    /// <summary>
    /// Идентификатор подкатегории
    /// </summary>
    public int SubcategoryId { get; set; }
    /// <summary>
    /// Подкатегория
    /// </summary>
    public Subсategory? Subcategory { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// Параметры
    /// </summary>
    public List<ProductParameter>? Parameters { get; set; }
    /// <summary>
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Скидки
    /// </summary>
    public List<Discount>? Discounts { get; set; }
}
