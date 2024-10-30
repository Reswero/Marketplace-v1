using Marketplace.Common.Outbox.Queue;
using Marketplace.Products.Application.Common;
using Marketplace.Products.Application.Common.DTOs;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Marketplace.Products.Infrastructure.Products.Services;

/// <summary>
/// Объектное хранилище
/// </summary>
internal class ProductsObjectStorage(ILogger<ProductsObjectStorage> logger, IMinioClient client,
    IOutboxQueue<string> outbox)
    : IProductsObjectStorage
{
    private readonly ILogger<ProductsObjectStorage> _logger = logger;
    private readonly IMinioClient _client = client;
    private readonly IOutboxQueue<string> _outbox = outbox;

    /// <inheritdoc/>
    public async Task<List<string>> UploadImagesAsync(List<FileDto> images, CancellationToken cancellationToken = default)
    {
        List<string> imageNames = new(images.Count);

        foreach (var image in images)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string fileName = $"{guid}.jpg";

            PutObjectArgs args = new PutObjectArgs()
                .WithBucket(MarketplaceConsts.ProductsImagesBucket)
                .WithObject(fileName)
                .WithObjectSize(image.Stream.Length)
                .WithStreamData(image.Stream)
                .WithContentType(image.ContentType);

            var response = await _client.PutObjectAsync(args, cancellationToken);

            if (string.IsNullOrWhiteSpace(response.Etag))
            {
                // TODO: Add images to outbox to delete
                throw new UploadImagesException();
            }

            imageNames.Add(fileName);
        }

        return imageNames;
    }

    /// <inheritdoc/>
    public async Task DeleteImagesAsync(List<string> imageNames, CancellationToken cancellationToken = default)
    {
        RemoveObjectsArgs removeArgs = new RemoveObjectsArgs()
            .WithBucket(MarketplaceConsts.ProductsImagesBucket)
            .WithObjects(imageNames);

        try
        {
            await _client.RemoveObjectsAsync(removeArgs, cancellationToken);
        }
        catch (Exception e)
        {
            await _outbox.PushAsync([.. imageNames], cancellationToken);
            _logger.LogError(e, "Ошибка во время удаления изображений из объектного хранилища. {Error}", e.Message);
        }
    }
}
