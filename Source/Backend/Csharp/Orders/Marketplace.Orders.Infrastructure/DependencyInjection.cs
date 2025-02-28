using Marketplace.Common.Identity.User;
using Marketplace.Common.Transactions;
using Marketplace.Orders.Application.Common.Interfaces;
using Marketplace.Orders.Infrastructure.Common.Persistence;
using Marketplace.Orders.Infrastructure.Integrations.Products.Services;
using Marketplace.Orders.Infrastructure.Orders.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddDatabase(configuration);
        services.AddUserIdentity();
        services.AddServicesClients(configuration);
        services.AddRepositories();

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

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IOrderProductsRepository, OrderProductsRepository>();

        return services;
    }
}
