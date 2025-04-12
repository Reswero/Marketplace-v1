using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marketplace.Delivery.Infrastructure.Deliveries.Services;

/// <summary>
/// Фоновый сервис для обработки доставок заказов
/// </summary>
/// <param name="logger"></param>
/// <param name="provider"></param>
internal class ProcessingDeliveriesBackgroundService(ILogger<ProcessingDeliveriesBackgroundService> logger,
    IServiceProvider provider)
    : BackgroundService
{
    private readonly ILogger<ProcessingDeliveriesBackgroundService> _logger = logger;
    private readonly IServiceProvider _provider = provider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var worker = scope.ServiceProvider.GetRequiredService<ProcessingDeliveriesWorker>();

                await worker.ExecuteAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка во время обработки доставок заказов. {Error}", e.Message);
            }

            var nextDelayInSeconds = Random.Shared.Next(30, 120);
            await Task.Delay(TimeSpan.FromSeconds(nextDelayInSeconds), stoppingToken);
        }
    }
}
