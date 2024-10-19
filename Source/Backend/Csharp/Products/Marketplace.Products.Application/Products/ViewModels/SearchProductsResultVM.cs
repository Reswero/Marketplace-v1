namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель результата поиска товаров
/// </summary>
/// <param name="TotalCount">Всего товаров (отображает до 1000)</param>
/// <param name="Products">Товары</param>
public record SearchProductsResultVM(int TotalCount, List<ProductShortInfoVM> Products);
