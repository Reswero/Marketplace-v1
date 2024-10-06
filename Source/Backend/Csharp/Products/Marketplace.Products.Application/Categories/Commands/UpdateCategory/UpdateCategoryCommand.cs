using Marketplace.Products.Application.Categories.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Команда обновления категории
/// </summary>
/// <param name="Name">Название</param>
/// <param name="Parameters">Параметры</param>
public record UpdateCategoryCommand(string Name, List<UpdateCategoryParameterVM> Parameters);

/// <summary>
/// Команда обновления категории
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
/// <param name="Parameters">Параметры</param>
public record UpdateCategoryWithIdCommand(int Id, string Name, List<UpdateCategoryParameterVM> Parameters)
    : IRequest;
