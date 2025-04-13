using Marketplace.Common.Transactions;
using Marketplace.Delivery.Application.Common.Interfaces;
using Marketplace.Delivery.Infrastructure.Common.Persistence;
using Marketplace.Delivery.Infrastructure.Deliveries.Persistence;
using Marketplace.Delivery.Infrastructure.Deliveries.Services;
using Marketplace.Delivery.Infrastructure.Integrations.Orders;
using Marketplace.Orders.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Delivery.Infrastructure;

/// <summary>
/// Внедрение зависимостей слоя инфраструктуры
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить слой инфраструктуры
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Конфигурация</param>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddServicesClients(configuration);
        services.AddWorkers();

        return services;
    }

    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<DeliveriesContext>();

        db.Database.EnsureCreated();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<DeliveriesContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<DeliveriesContext>());

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderDeliveriesRepository, OrderDeliveriesRepository>();
        return services;
    }
    
    private static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddScoped<ProcessingDeliveriesWorker>();
        services.AddHostedService<ProcessingDeliveriesBackgroundService>();

        return services;
    }

    private static IServiceCollection AddServicesClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrdersServiceClient, OrdersServiceClient>();
        services.AddGrpcClient<OrdersService.OrdersServiceClient>(options =>
        {
            var address = configuration["Services:Orders:Address"]!;
            options.Address = new Uri(address);
        });

        return services;
    }
}
