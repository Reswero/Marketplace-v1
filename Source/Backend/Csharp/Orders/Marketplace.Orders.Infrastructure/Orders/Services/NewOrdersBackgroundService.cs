using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marketplace.Orders.Infrastructure.Orders.Services;

/// <summary>
/// Фоновый сервис для работы с новыми заказами
/// </summary>
/// <param name="provider"></param>
internal class NewOrdersBackgroundService(IServiceProvider provider)
    : BackgroundService
{
    private readonly IServiceProvider _provider = provider;
    private readonly TimeSpan _delayBetweenProcessing = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            using var scope = _provider.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<NewOrdersWorker>();

            await worker.ExecuteAsync(stoppingToken);

            await Task.Delay(_delayBetweenProcessing, stoppingToken);
        }
    }
}
