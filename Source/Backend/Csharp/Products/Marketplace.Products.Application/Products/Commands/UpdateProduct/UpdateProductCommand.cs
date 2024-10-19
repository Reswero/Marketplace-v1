using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.UpdateProduct;

/// <summary>
/// Команда обновления товара
/// </summary>
/// <param name="SubcategoryId">Идентификатор подкатегории</param>
/// <param name="Name">Название</param>
/// <param name="Description">Описание</param>
/// <param name="Price">Цена</param>
/// <param name="Parameters">Параметры</param>
public record UpdateProductCommand(int SubcategoryId, string Name,
    string Description, int Price, List<UpdateProductParameterVM> Parameters);

/// <summary>
/// Команда обновления товара
/// </summary>
/// <param name="Id">Идентификатор товара</param>
/// <param name="SubcategoryId">Идентификатор подкатегории</param>
/// <param name="Name">Название</param>
/// <param name="Description">Описание</param>
/// <param name="Price">Цена</param>
/// <param name="Parameters">Параметры</param>
public record UpdateProductWithIdCommand(int Id, int SubcategoryId, string Name,
    string Description, int Price, List<UpdateProductParameterVM> Parameters)
    :IRequest;
