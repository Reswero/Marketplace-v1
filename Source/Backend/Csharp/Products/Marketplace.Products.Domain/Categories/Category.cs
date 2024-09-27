namespace Marketplace.Products.Domain.Categories;

/// <summary>
/// Категория
/// </summary>
public class Category
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Промокоды
    /// </summary>
    public List<Promocode>? Promocodes { get; set; }
}
