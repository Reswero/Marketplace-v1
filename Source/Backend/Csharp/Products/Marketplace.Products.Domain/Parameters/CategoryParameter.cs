using Marketplace.Products.Domain.Categories;

namespace Marketplace.Products.Domain.Parameters;

/// <summary>
/// Параметр категории
/// </summary>
public class CategoryParameter
{
    private CategoryParameter() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="category">Категория</param>
    /// <param name="name">Название</param>
    /// <param name="type">Тип</param>
    public CategoryParameter(Category category, string name, ParameterType type)
    {
        Category = category;
        Name = name;
        Type = type;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <param name="id">Идентификатор</param>
    /// <param name="name">Название</param>
    /// <param name="type">Тип</param>
    public CategoryParameter(int categoryId, int id, string name, ParameterType type)
    {
        CategoryId = categoryId;
        Id = id;
        Name = name;
        Type = type;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Категория
    /// </summary>
    public Category? Category { get; set; } = null;
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Тип
    /// </summary>
    public ParameterType Type { get; set; }
}
