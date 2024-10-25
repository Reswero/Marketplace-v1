using Marketplace.Products.Application.Common.DTOs;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Marketplace.Products.Infrastructure.Products.Services;

/// <summary>
/// Объектное хранилище
/// </summary>
internal class ProductsObjectStorage(ILogger<ProductsObjectStorage> logger, IMinioClient client)
    : IProductsObjectStorage
{
    private const string _bucketName = "products-bucket";

    private readonly ILogger<ProductsObjectStorage> _logger = logger;
    private readonly IMinioClient _client = client;

    /// <inheritdoc/>
    public async Task<ImagesUploadResult> UploadImagesAsync(List<FileDto> images, CancellationToken cancellationToken = default)
    {
        bool hasError = false;
        List<string> imageNames = new(images.Count);

        foreach (var image in images)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string fileName = $"{guid}.jpg";

            PutObjectArgs args = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithObjectSize(image.Stream.Length)
                .WithStreamData(image.Stream)
                .WithContentType(image.ContentType);

            var response = await _client.PutObjectAsync(args, cancellationToken);

            if (string.IsNullOrWhiteSpace(response.Etag))
            {
                hasError = false;
                break;
            }

            imageNames.Add(fileName);
        }

        return new(hasError, imageNames);
    }

    /// <inheritdoc/>
    public async Task DeleteImagesAsync(List<string> imageNames, CancellationToken cancellationToken = default)
    {
        RemoveObjectsArgs removeArgs = new RemoveObjectsArgs()
            .WithBucket(_bucketName)
            .WithObjects(imageNames);

        try
        {
            await _client.RemoveObjectsAsync(removeArgs, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время удаления изображений из объектного хранилища. {Error}", e.Message);
        }
    }
}
