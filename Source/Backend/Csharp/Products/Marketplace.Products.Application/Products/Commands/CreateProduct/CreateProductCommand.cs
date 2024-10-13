using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.CreateProduct;

/// <summary>
/// Команда создания товара
/// </summary>
/// <param name="SellerId">Идентификатор продавца</param>
/// <param name="CategoryId">Идентификатор категории</param>
/// <param name="SubcategoryId">Идентификатор подкатегории</param>
/// <param name="Name">Название</param>
/// <param name="Description">Описание</param>
/// <param name="Price">Цена</param>
/// <param name="Parameters">Параметры</param>
public record CreateProductCommand(int SellerId, int CategoryId, int SubcategoryId,
    string Name, string Description, int Price, List<AddProductParameterVM> Parameters)
    :IRequest<CreateObjectResultVM>;