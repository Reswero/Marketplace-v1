using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Команда удаления категории
/// </summary>
/// <param name="Id">Идентификатор</param>
public record DeleteCategoryCommand(int Id) : IRequest<DeleteObjectResultVM>;
