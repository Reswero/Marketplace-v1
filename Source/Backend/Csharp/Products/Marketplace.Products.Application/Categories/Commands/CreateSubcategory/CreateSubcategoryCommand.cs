using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.CreateSubcategory;

/// <summary>
/// Команда создания подкатегории
/// </summary>
/// <param name="CategoryId">Идентификатор категории</param>
/// <param name="Name">Название</param>
public record CreateSubcategoryCommand(int CategoryId, string Name)
    : IRequest<CreateObjectResultVM>;
