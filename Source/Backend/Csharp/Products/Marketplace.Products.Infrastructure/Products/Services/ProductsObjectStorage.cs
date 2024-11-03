using Marketplace.Common.Outbox.Queue;
using Marketplace.Products.Application.Common.DTOs;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Products;
using Marketplace.Products.Infrastructure.Products.Models;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Marketplace.Products.Infrastructure.Products.Services;

/// <summary>
/// Объектное хранилище
/// </summary>
/// <param name="logger">Логгер</param>
/// <param name="storage">Клиент объектного хранилища</param>
/// <param name="outbox">Outbox с изображениями товаров</param>
/// <param name="imagesBucket">Бакет с изображениями товаров</param>
internal class ProductsObjectStorage(ILogger<ProductsObjectStorage> logger, IMinioClient storage,
    IOutboxQueue<ImageToDelete> outbox, string imagesBucket)
    : IProductsObjectStorage
{
    private readonly string _imagesBucket = imagesBucket;

    private readonly ILogger<ProductsObjectStorage> _logger = logger;
    private readonly IMinioClient _storage = storage;
    private readonly IOutboxQueue<ImageToDelete> _outbox = outbox;

    /// <inheritdoc/>
    public async Task<List<string>> UploadImagesAsync(List<FileDto> images, CancellationToken cancellationToken = default)
    {
        List<string> imageNames = new(images.Count);

        foreach (var image in images)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string fileName = $"{guid}.jpg";

            PutObjectArgs args = new PutObjectArgs()
                .WithBucket(_imagesBucket)
                .WithObject(fileName)
                .WithObjectSize(image.Stream.Length)
                .WithStreamData(image.Stream)
                .WithContentType(image.ContentType);

            var response = await _storage.PutObjectAsync(args, cancellationToken);

            if (string.IsNullOrWhiteSpace(response.Etag))
            {
                var toDelete = images.Select(i => new ImageToDelete(_imagesBucket, i.Name)).ToArray();
                await _outbox.PushAsync(toDelete, CancellationToken.None);

                throw new UploadImagesException();
            }

            imageNames.Add(fileName);
        }

        return imageNames;
    }

    /// <inheritdoc/>
    public async Task DeleteImagesAsync(IReadOnlyCollection<Image> images, CancellationToken cancellationToken = default)
    {
        var imagesByBuckets = images.GroupBy(i => i.BucketName).ToList();

        try
        {
            while (imagesByBuckets.Count > 0)
            {
                var bucketImages = imagesByBuckets[0];
                var imagesNames = bucketImages.Select(i => i.Name).ToList();

                RemoveObjectsArgs removeArgs = new RemoveObjectsArgs()
                    .WithBucket(bucketImages.Key)
                    .WithObjects(imagesNames);

                await _storage.RemoveObjectsAsync(removeArgs, cancellationToken);
                imagesByBuckets.Remove(bucketImages);
            }
        }
        catch (Exception e)
        {
            List<ImageToDelete> toDelete = [];
            foreach (var bucketImages in imagesByBuckets)
            {
                foreach (var image in bucketImages)
                {
                    toDelete.Add(new(image.BucketName, image.Name));
                }
            }

            await _outbox.PushAsync([.. toDelete], cancellationToken);
            _logger.LogError(e, "Ошибка во время удаления изображений из объектного хранилища. {Error}", e.Message);
        }
    }
}
