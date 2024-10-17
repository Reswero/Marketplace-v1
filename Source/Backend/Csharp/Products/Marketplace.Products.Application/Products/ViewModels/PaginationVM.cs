namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель пагинации
/// </summary>
/// <param name="Offset">Смещение</param>
/// <param name="Limit">Количество элементов</param>
public record PaginationVM(int Offset, int Limit);
