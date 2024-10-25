namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Моодель товара с краткой информацией
/// </summary>
public record ProductShortInfoVM
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
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Скидка
    /// </summary>
    public DiscountVM? Discount { get; set; }
    /// <summary>
    /// Изображение
    /// </summary>
    public string? Image { get; set; }
    /// <summary>
    /// Статус
    /// </summary>
    public ProductStatus Status { get; set; }
}
