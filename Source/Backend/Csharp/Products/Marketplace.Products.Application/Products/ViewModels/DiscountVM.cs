namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель скидки
/// </summary>
/// <param name="Size">Размер</param>
/// <param name="ValidUntil">Действительна до</param>
public record DiscountVM(int Size, DateOnly ValidUntil);
