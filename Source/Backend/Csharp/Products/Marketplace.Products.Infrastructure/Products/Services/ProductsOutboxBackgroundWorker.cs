using Marketplace.Common.Outbox.Queue;
using Marketplace.Products.Infrastructure.Products.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Marketplace.Products.Infrastructure.Products.Services;

/// <summary>
/// Обработчик outbox'а изображений товаров
/// </summary>
/// <param name="logger">Логгер</param>
/// <param name="storage">Клиент объектного хранилища</param>
/// <param name="outbox">Outbox с изображениями</param>
internal class ProductsOutboxBackgroundWorker(ILogger<ProductsOutboxBackgroundWorker> logger,
    IMinioClient storage, IOutboxQueue<ImageToDelete> outbox)
    : BackgroundService
{
    private const int _packetSize = 1_000;

    private readonly TimeSpan _executionDelay = TimeSpan.FromSeconds(300);

    private readonly ILogger<ProductsOutboxBackgroundWorker> _logger = logger;
    private readonly IMinioClient _storage = storage;
    private readonly IOutboxQueue<ImageToDelete> _outbox = outbox;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DeleteImages(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка во время попытки удаления изображений из объектного хранилища. {Error}", e.Message);
            }

            await Task.Delay(_executionDelay, stoppingToken);
        }
    }

    private async Task DeleteImages(CancellationToken cancellationToken)
    {
        var images = await _outbox.PeekAsync(_packetSize, cancellationToken);
        
        foreach (var image in images)
        {
            RemoveObjectArgs removeArgs = new RemoveObjectArgs()
                .WithBucket(image.Bucket)
                .WithObject(image.Name);

            await _storage.RemoveObjectAsync(removeArgs, cancellationToken);
            await _outbox.PopAsync(cancellationToken);
        }
    }
}
