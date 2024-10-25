namespace Marketplace.Products.Application.Common.Models;

/// <summary>
/// Результат загрузки изображений
/// </summary>
/// <param name="HasError">Произошла ли ошибка при загрузке</param>
/// <param name="UploadedImagesNames">Названия загруженных изображений</param>
public record ImagesUploadResult(bool HasError, List<string> UploadedImagesNames);
