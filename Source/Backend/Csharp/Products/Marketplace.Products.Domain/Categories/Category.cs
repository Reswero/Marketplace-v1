using Marketplace.Common.SoftDelete;
using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Domain.Categories;

/// <summary>
/// Категория
/// </summary>
public class Category : ISoftDelete
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
    /// Параметры
    /// </summary>
    public List<CategoryParameter>? Parameters { get; set; }
    /// <summary>
    /// Промокоды
    /// </summary>
    public List<Promocode>? Promocodes { get; set; }
    /// <summary>
    /// Подкатегории
    /// </summary>
    public List<Subсategory>? Subсategories { get; set; }
    /// <inheritdoc/>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <inheritdoc/>
    public void SetDeleted()
    {
        DeletedAt = DateTimeOffset.Now;
    }
}
