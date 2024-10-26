using MediatR;

namespace Marketplace.Products.Application.Products.Commands.DeleteImages;

/// <summary>
/// Модель удаления изображений товара
/// </summary>
/// <param name="ImagesIds">Идентификаторы изображений</param>
public record DeleteImagesCommand(HashSet<int> ImagesIds)
    : IRequest;

/// <summary>
/// Модель удаления изображений товара
/// </summary>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="ImagesIds">Идентификаторы изображений</param>
public record DeleteImagesWithProductIdCommand(int ProductId, HashSet<int> ImagesIds)
    : IRequest;
