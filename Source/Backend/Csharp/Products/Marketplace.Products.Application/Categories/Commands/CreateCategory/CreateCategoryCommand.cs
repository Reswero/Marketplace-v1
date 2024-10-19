using Marketplace.Products.Application.Categories.ViewModels;
using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.CreateCategory;

/// <summary>
/// Команда создания категории
/// </summary>
/// <param name="Name">Название</param>
/// <param name="Parameters">Параметры</param>
public record CreateCategoryCommand(string Name, List<AddCategoryParamterVM> Parameters)
    : IRequest<CreateObjectResultVM>;
