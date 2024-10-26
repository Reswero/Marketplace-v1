using Marketplace.Products.Application.Categories.ViewModels;
using Marketplace.Products.Application.Users.ViewModels;

namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель товара
/// </summary>
public record ProductVM
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Продавец
    /// </summary>
    public SellerVM? Seller { get; set; }
    /// <summary>
    /// Категория
    /// </summary>
    public ShortCategoryVM Category { get; set; } = null!;
    /// <summary>
    /// Подкатегория
    /// </summary>
    public SubcategoryVM Subcategory { get; set; } = null!;
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Скидка
    /// </summary>
    public DiscountVM? Discount { get; set; }
    /// <summary>
    /// Параметры
    /// </summary>
    public List<ProductParameterVM> Parameters { get; set; } = null!;
    /// <summary>
    /// Изображения
    /// </summary>
    public List<string> Images { get; set; } = null!;
    /// <summary>
    /// Статус
    /// </summary>
    public ProductStatus Status { get; set; }
}
