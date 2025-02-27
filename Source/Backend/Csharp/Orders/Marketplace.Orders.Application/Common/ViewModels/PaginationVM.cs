namespace Marketplace.Orders.Application.Common.ViewModels;

/// <summary>
/// Модель пагинации
/// </summary>
/// <param name="Offset">Смещение</param>
/// <param name="Limit">Ограничение</param>
public record PaginationVM(int Offset, int Limit);
