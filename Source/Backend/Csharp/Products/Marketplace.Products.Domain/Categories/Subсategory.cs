namespace Marketplace.Products.Domain.Categories;

/// <summary>
/// Подкатегория
/// </summary>
public class Subсategory
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
}
