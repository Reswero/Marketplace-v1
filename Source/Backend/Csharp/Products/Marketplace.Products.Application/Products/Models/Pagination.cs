namespace Marketplace.Products.Application.Products.Models;

/// <summary>
/// Пагинация
/// </summary>
/// <param name="Offset">Смещение</param>
/// <param name="Limit">Количество элементов</param>
public record Pagination(int Offset = 0, int Limit = 50);
