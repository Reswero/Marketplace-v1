namespace Marketplace.Products.Domain.Categories;

/// <summary>
/// Промокод
/// </summary>
public class Promocode
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Действителен до
    /// </summary>
    public DateOnly ValidUntil { get; set; }
}
