namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель изображения
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Order">Порядок</param>
/// <param name="Name">Название</param>
public record ImageVM(int Id, int Order, string Name);
