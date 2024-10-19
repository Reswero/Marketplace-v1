using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Application.Categories.ViewModels;

/// <summary>
/// Модель добавления параметра категории
/// </summary>
/// <param name="Name">Название</param>
/// <param name="Type">Тип</param>
public record AddCategoryParamterVM(string Name, ParameterType Type);
