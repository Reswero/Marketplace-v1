using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.DeleteSubcategory;

/// <summary>
/// Команда удаления подкатегории
/// </summary>
/// <param name="CategoryId">Идентификатор категории</param>
/// <param name="Id">Идентификатор</param>
public record DeleteSubcategoryCommand(int CategoryId, int Id)
    : IRequest<DeleteObjectResultVM>;
