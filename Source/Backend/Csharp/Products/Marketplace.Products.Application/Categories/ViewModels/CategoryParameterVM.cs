using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Application.Categories.ViewModels;

/// <summary>
/// Модель параметра категории
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
/// <param name="Type">Тип</param>
public record CategoryParameterVM(int Id, string Name, ParameterType Type);
