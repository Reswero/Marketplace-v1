using Marketplace.Products.Application.Common.DTOs;

namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Объектное хранилище
/// </summary>
public interface IProductsObjectStorage
{
    /// <summary>
    /// Загрузить изображения товара
    /// </summary>
    /// <param name="images">Изображения</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Названия загруженных изображений</returns>
    public Task<List<string>> UploadImagesAsync(List<FileDto> images, CancellationToken cancellationToken = default);
    /// <summary>
    /// Удалить изображения
    /// </summary>
    /// <param name="imageNames">Названия изображений</param>
    /// <param name="cancellationToken"></param>
    public Task DeleteImagesAsync(List<string> imageNames, CancellationToken cancellationToken = default);
}
