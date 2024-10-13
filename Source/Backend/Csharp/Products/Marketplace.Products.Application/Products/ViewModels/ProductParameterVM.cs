namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель параметра товара
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="CategoryParameterId">Идентификатор параметра категории</param>
/// <param name="Name">Название</param>
/// <param name="Value">Значение</param>
public record ProductParameterVM(int Id, int CategoryParameterId, string Name, string Value);
