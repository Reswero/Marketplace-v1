namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель фильтра по цене
/// </summary>
/// <param name="From">От</param>
/// <param name="To">До</param>
public record PriceFilterVM(int From, int? To);
