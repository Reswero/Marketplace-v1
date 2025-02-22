using Marketplace.Products.Application.Categories.ViewModels;
using Marketplace.Products.Application.Products.Models;

namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель товара
/// </summary>
public record ProductFullInfoVM
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
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
    /// Скидки
    /// </summary>
    public List<DiscountVM>? Discounts { get; set; }
    /// <summary>
    /// Параметры
    /// </summary>
    public List<ProductParameterVM> Parameters { get; set; } = null!;
    /// <summary>
    /// Изображения
    /// </summary>
    public List<ImageVM> Images { get; set; } = null!;
    /// <summary>
    /// Статус
    /// </summary>
    public ProductStatus Status { get; set; }
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}