using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Services;

/// <summary>
/// Фоновый сервис обработки новых статустов доставки
/// </summary>
/// <param name="logger"></param>
/// <param name="provider"></param>
internal class NewDeliveryStatusOutboxBackgroundService(
    ILogger<NewDeliveryStatusOutboxBackgroundService> logger, IServiceProvider provider)
    : BackgroundService
{
    private readonly ILogger<NewDeliveryStatusOutboxBackgroundService> _logger = logger;
    private readonly IServiceProvider _provider = provider;

    private readonly TimeSpan _executionDelay = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var worker = scope.ServiceProvider.GetRequiredService<NewDeliveryStatusOutboxWorker>();

                await worker.ExecuteAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка во время обработки outbox'а со статусами. {Error}", e.Message);
            }

            await Task.Delay(_executionDelay, stoppingToken);
        }
    }
}
