using Marketplace.Common.SoftDelete;
using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Domain.Categories;

/// <summary>
/// Категория
/// </summary>
public class Category : ISoftDelete
{
    private readonly List<CategoryParameter> _parameters = [];

    private Category() { }

    public Category(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; private set; } = null!;
    /// <summary>
    /// Параметры
    /// </summary>
    public IReadOnlyList<CategoryParameter> Parameters => _parameters;
    /// <summary>
    /// Промокоды
    /// </summary>
    public List<Promocode> Promocodes { get; private set; } = [];
    /// <summary>
    /// Подкатегории
    /// </summary>
    public List<Subсategory> Subсategories { get; private set; } = [];
    /// <inheritdoc/>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <inheritdoc/>
    public void SetDeleted()
    {
        DeletedAt = DateTimeOffset.Now;
    }

    /// <summary>
    /// Установить название
    /// </summary>
    /// <param name="newName">Новое название</param>
    public void SetName(string newName)
    {
        Name = newName;
    }

    /// <summary>
    /// Добавить параметры
    /// </summary>
    /// <param name="parameters">Параметры</param>
    public void AddParameters(params CategoryParameter[] parameters)
    {
        if (parameters.Length > 0)
            _parameters.AddRange(parameters);
    }

    /// <summary>
    /// Удалить параметры
    /// </summary>
    /// <param name="parameters">Параметры</param>
    public void RemoveParameters(params CategoryParameter[] parameters)
    {
        foreach (var parameter in parameters)
        {
            _parameters.Remove(parameter);
        }
    }
}
