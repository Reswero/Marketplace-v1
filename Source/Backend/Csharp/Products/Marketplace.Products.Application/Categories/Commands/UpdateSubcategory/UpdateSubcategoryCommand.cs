using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.UpdateSubcategory;

/// <summary>
/// Команда обновления подкатегории
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
public record UpdateSubcategoryCommand(int Id, string Name);

/// <summary>
/// Команда обновления подкатегории
/// </summary>
/// <param name="CategoryId">Идентификатор категории</param>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
public record UpdateSubcategoryWithCategoryIdCommand(int CategoryId, int Id, string Name)
    :IRequest;
