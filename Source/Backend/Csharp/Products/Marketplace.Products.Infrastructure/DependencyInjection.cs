using Marketplace.Common.Outbox.Queue;
using Marketplace.Common.SoftDelete;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Infrastructure.Categories.Persistence;
using Marketplace.Products.Infrastructure.Common.Persistence;
using Marketplace.Products.Infrastructure.Common.Services;
using Marketplace.Products.Infrastructure.Integrations.Favorites;
using Marketplace.Products.Infrastructure.Products.Models;
using Marketplace.Products.Infrastructure.Products.Services;
using Marketplace.Products.Infrastructure.Users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Serilog;

namespace Marketplace.Products.Infrastructure;

/// <summary>
/// Инъекция зависимостей слоя инфраструктуры
/// </summary>
public static class DependencyInjection
{
    private const string _productsOutbox = "products_outbox.db";

    /// <summary>
    /// Добавить слой инфраструктуры
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Конфигурация</param>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        // Логгирование
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });

        // База данных
        services.AddDbContext<ProductsContext>(opt =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            opt.UseNpgsql(connectionString);
            opt.AddInterceptors(new SoftDeleteInterceptor());
        });

        // Объектное хранилище
        services.AddMinio(client =>
        {
            client.WithEndpoint(configuration["ObjectStorage:Connection:Address"])
                .WithCredentials(configuration["ObjectStorage:Connection:Login"], configuration["ObjectStorage:Connection:Password"])
                .WithSSL(Convert.ToBoolean(configuration["ObjectStorage:Connection:UseSSL"]))
                .Build();
        });

        // Общие сервисы
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserIdentityProvider, HttpUserIdentityProvider>();
        
        // Репозитории
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ProductsContext>());
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        // Outbox'ы
        services.AddKeyedSingleton<IOutboxQueue<ImageToDelete>>(_productsOutbox, (_, _) =>
        {
            return new OutboxQueue<ImageToDelete>(_productsOutbox, true);
        });

        // Сервисы товаров
        services.AddScoped<IProductsAccessChecker, ProductsAccessChecker>();
        services.AddScoped<IProductsSearcher, ProductsSearcher>();
        services.AddScoped<IProductsObjectStorage>(provider =>
        {
            var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ProductsObjectStorage>>();
            var minioClient = provider.GetRequiredService<IMinioClient>();
            var outbox = provider.GetRequiredKeyedService<IOutboxQueue<ImageToDelete>>(_productsOutbox);
            var imagesBucket = configuration["ObjectStorage:Buckets:ProductsImages"]!;

            return new ProductsObjectStorage(logger, minioClient, outbox, imagesBucket);
        });

        // Внешние сервисы
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IFavoritesService, FavoritesService>();

        services.AddHttpClient<IUsersService, UsersService>(client =>
        {
            var address = configuration["Services:Users:Address"]!;
            var timeout = int.Parse(configuration["Services:Users:Timeout"]!);

            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
        });

        services.AddHttpClient<IFavoritesService, FavoritesService>(client =>
        {
            var address = configuration["Services:Favorites:Address"]!;
            var timeout = int.Parse(configuration["Services:Favorites:Timeout"]!);

            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
        });

        // Бэкграунд сервисы
        services.AddHostedService(provider =>
        {
            var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ProductsOutboxBackgroundWorker>>();
            var minioClient = provider.GetRequiredService<IMinioClient>();
            var outbox = provider.GetRequiredKeyedService<IOutboxQueue<ImageToDelete>>(_productsOutbox);

            return new ProductsOutboxBackgroundWorker(logger, minioClient, outbox);
        });

        return services;
    }

    /// <summary>
    /// Инициализировать базу данных
    /// </summary>
    /// <param name="provider"></param>
    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();

        db.Database.EnsureCreated();
    }
}
