using Marketplace.Products.Application.Common.DTOs;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.UploadImages;

/// <summary>
/// Модель загрузки изображений в товар
/// </summary>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="Images">Изображения</param>
public record UploadImagesCommand(int ProductId, List<FileDto> Images)
    : IRequest;
