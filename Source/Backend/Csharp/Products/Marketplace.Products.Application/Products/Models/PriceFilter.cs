namespace Marketplace.Products.Application.Products.Models;

/// <summary>
/// Фильтр цены
/// </summary>
/// <param name="From">От</param>
/// <param name="To">До</param>
public record PriceFilter(int From = 0, int? To = null);
