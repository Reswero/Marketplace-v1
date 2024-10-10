using Marketplace.Products.Domain.Products;

namespace Marketplace.Products.Domain.Parameters;

/// <summary>
/// Параметр товара
/// </summary>
public class ProductParameter
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор параметра категории
    /// </summary>
    public int CategoryParameterId { get; set; }
    /// <summary>
    /// Параметр категории
    /// </summary>
    public CategoryParameter? CategoryParameter { get; set; }
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// Товар
    /// </summary>
    public Product? Product { get; set; }
    /// <summary>
    /// Значение
    /// </summary>
    public string Value { get; set; } = null!;
}
