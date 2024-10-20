using Marketplace.Common.SoftDelete;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Domain.Products;

/// <summary>
/// Товар
/// </summary>
public class Product : ISoftDelete
{
    private readonly List<ProductParameter> _parameters = [];
    private readonly List<Discount> _discounts = [];
    private readonly List<Image> _images = []; 

    private Product() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="sellerId">Идентификатор продавца</param>
    /// <param name="category">Категория</param>
    /// <param name="subсategory">Подкатегория</param>
    /// <param name="name">Название</param>
    /// <param name="description">Описание</param>
    /// <param name="price">Цена</param>
    public Product(int sellerId, Category category, Subсategory subсategory,
        string name, string description, int price)
    {
        SellerId = sellerId;
        Category = category;
        Subcategory = subсategory;
        Name = name;
        Description = description;
        Price = price;

        CreatedAt = DateTimeOffset.UtcNow;
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
    public List<Discount> Discounts => _discounts;
    /// <summary>
    /// Изображения
    /// </summary>
    public List<Image> Images => _images;
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
    /// <inheritdoc/>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <inheritdoc/>
    public void SetDeleted()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }

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

        SetUpdated();
    }

    /// <summary>
    /// Изменить подкатегорию
    /// </summary>
    /// <param name="subсategory">Подкатегория</param>
    public void ChangeSubcategory(Subсategory subсategory)
    {
        Subcategory = subсategory;
        SubcategoryId = subсategory.Id;

        SetUpdated();
    }

    /// <summary>
    /// Добавить параметры
    /// </summary>
    /// <param name="parameters">Параметры</param>
    public void AddParameters(params ProductParameter[] parameters)
    {
        if (parameters.Length > 0)
            _parameters.AddRange(parameters);

        SetUpdated();
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

        SetUpdated();
    }

    /// <summary>
    /// Добавить скидку
    /// </summary>
    /// <param name="discount">Скидка</param>
    public void AddDiscount(Discount discount)
    {
        _discounts.Add(discount);
        SetUpdated();
    }

    /// <summary>
    /// Удалить скидку
    /// </summary>
    /// <param name="discount">Скидка</param>
    public void RemoveDiscount(Discount discount)
    {
        _discounts.Remove(discount);
        SetUpdated();
    }

    /// <summary>
    /// Добавить изображения
    /// </summary>
    /// <param name="images">Изображения</param>
    public void AddImages(params Image[] images)
    {
        if (images.Length > 0)
            _images.AddRange(images);

        SetUpdated();
    }

    /// <summary>
    /// Удалить изображения
    /// </summary>
    /// <param name="images">Изображения</param>
    public void RemoveImages(params Image[] images)
    {
        foreach (var image in images)
        {
            _images.Remove(image);
        }
    }

    /// <summary>
    /// Пометить товар как обновленный
    /// </summary>
    private void SetUpdated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
