using Marketplace.Products.Domain.Products;

namespace Marketplace.Products.Domain.Parameters;

/// <summary>
/// Параметр товара
/// </summary>
public class ProductParameter
{
    private ProductParameter() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="categoryParameter">Параметр категории</param>
    /// <param name="product">Товар</param>
    /// <param name="value">Значение</param>
    public ProductParameter(CategoryParameter categoryParameter, Product product, string value)
    {
        CategoryParameter = categoryParameter;
        Product = product;
        Value = value;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="categoryParameterId">Идентификатор параметра категории</param>
    /// <param name="productId">Идентификатор товара</param>
    /// <param name="value">Значение</param>
    public ProductParameter(int id, int categoryParameterId, int productId, string value)
    {
        Id = id;
        CategoryParameterId = categoryParameterId;
        ProductId = productId;
        Value = value;
    }

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
