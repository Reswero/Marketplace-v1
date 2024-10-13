using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Domain.Products;

/// <summary>
/// Товар
/// </summary>
public class Product
{
    private readonly List<ProductParameter> _parameters = [];

    private Product() { }

    public Product(int sellerId, Category category, Subсategory subсategory,
        string name, string description, int price)
    {
        SellerId = sellerId;
        Category = category;
        Subcategory = subсategory;
        Name = name;
        Description = description;
        Price = price;
    }

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
    public List<ProductParameter> Parameters => _parameters;
    /// <summary>
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Скидки
    /// </summary>
    public List<Discount>? Discounts { get; set; }

    /// <summary>
    /// Изменить информацию
    /// </summary>
    /// <param name="name">Название</param>
    /// <param name="description">Описание</param>
    /// <param name="price">Цена</param>
    public void ChangeInfo(string name, string description, int price)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    /// <summary>
    /// Изменить подкатегорию
    /// </summary>
    /// <param name="subсategory">Подкатегория</param>
    public void ChangeSubcategory(Subсategory subсategory)
    {
        Subcategory = subсategory;
        SubcategoryId = subсategory.Id;
    }

    /// <summary>
    /// Добавить параметры
    /// </summary>
    /// <param name="parameters">Параметры</param>
    public void AddParameters(params ProductParameter[] parameters)
    {
        if (_parameters.Count > 0)
            _parameters.AddRange(parameters);
    }

    /// <summary>
    /// Удалить параметры
    /// </summary>
    /// <param name="parameters">Параметры</param>
    public void RemoveParameters(params ProductParameter[] parameters)
    {
        foreach (var parameter in parameters)
        {
            _parameters.Remove(parameter);
        }
    }
}
