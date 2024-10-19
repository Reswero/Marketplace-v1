using Marketplace.Common.SoftDelete;

namespace Marketplace.Products.Domain.Categories;

/// <summary>
/// Подкатегория
/// </summary>
public class Subсategory : ISoftDelete
{
    private Subсategory() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <param name="name">Название</param>
    public Subсategory(int categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public int CategoryId { get; private set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; private set; } = null!;

    public DateTimeOffset? DeletedAt {  get; private set; }

    /// <inheritdoc/>
    public void SetDeleted()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Установить название
    /// </summary>
    /// <param name="newName">Новое название</param>
    public void SetName(string newName)
    {
        Name = newName;
    }
}
