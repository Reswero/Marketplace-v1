namespace Marketplace.Products.Application.Products.ViewModels;

/// <summary>
/// Модель добавления параметра продукта
/// </summary>
/// <param name="CategoryParameterId">Идентификатор параметра категории</param>
/// <param name="Value">Значение</param>
public record AddProductParameterVM(int CategoryParameterId, string Value);
