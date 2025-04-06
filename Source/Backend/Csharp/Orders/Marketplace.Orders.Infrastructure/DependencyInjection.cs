using Grpc.Core;
using Grpc.Net.Client;
using Marketplace.Common.Identity.User;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Infrastructure.Common.Persistence;
using Marketplace.Orders.Infrastructure.Integrations.Payment;
using Marketplace.Orders.Infrastructure.Integrations.Products.Services;
using Marketplace.Orders.Infrastructure.Orders.Persistence;
using Marketplace.Orders.Infrastructure.Orders.Services;
using Marketplace.Payment.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading;

namespace Marketplace.Orders.Infrastructure;

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
        services.AddLogger(configuration);
        services.AddDatabase(configuration);
        services.AddUserIdentity();
        services.AddServicesClients(configuration);
        services.AddRepositories();
        services.AddOrdersWorkers();

        return services;
    }

    /// <summary>
    /// Инициализировать базу данных
    /// </summary>
    /// <param name="provider"></param>
    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrdersContext>();

        db.Database.EnsureCreated();
    }

    private static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<OrdersContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<OrdersContext>());

        return services;
    }

    private static IServiceCollection AddUserIdentity(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHttpUserIdentityProvider();

        return services;
    }

    private static IServiceCollection AddServicesClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductsServiceClient, ProductsServiceClient>();
        services.AddHttpClient<IProductsServiceClient, ProductsServiceClient>(client =>
        {
            var address = configuration["Services:Products:Address"]!;
            var timeout = int.Parse(configuration["Services:Products:Timeout"]!);

            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
        });

        services.AddScoped<IPaymentServiceClient, PaymentServiceClient>();
        services.AddGrpcClient<PaymentService.PaymentServiceClient>(options =>
        {
            var address = configuration["Services:Payment:Address"]!;
            options.Address = new Uri(address);
        }).ConfigureChannel((provider, options) =>
        {
            var timeout = int.Parse(configuration["Services:Payment:Timeout"]!);
            options.HttpClient!.Timeout = TimeSpan.FromMilliseconds(timeout);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IOrderProductsRepository, OrderProductsRepository>();
        services.AddScoped<INewOrdersRepository, NewOrdersRepository>();

        return services;
    }

    private static IServiceCollection AddOrdersWorkers(this IServiceCollection services)
    {
        services.AddScoped<NewOrdersWorker>();
        services.AddHostedService<NewOrdersBackgroundService>();

        return services;
    }
}
