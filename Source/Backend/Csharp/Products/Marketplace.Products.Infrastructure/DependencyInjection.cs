using Marketplace.Common.Outbox.Queue;
using Marketplace.Common.SoftDelete;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Infrastructure.Categories.Persistence;
using Marketplace.Products.Infrastructure.Common.Persistence;
using Marketplace.Products.Infrastructure.Products.Services;
using Marketplace.Products.Infrastructure.Users.Services;
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

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });

        services.AddDbContext<ProductsContext>(opt =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            opt.UseNpgsql(connectionString);
            opt.AddInterceptors(new SoftDeleteInterceptor());
        });

        services.AddMinio(client =>
        {
            client.WithEndpoint(configuration["ObjectStorage:Connection:Address"])
                .WithCredentials(configuration["ObjectStorage:Connection:Login"], configuration["ObjectStorage:Connection:Password"])
                .WithSSL(Convert.ToBoolean(configuration["ObjectStorage:Connection:UseSSL"]))
                .Build();
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ProductsContext>());
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        services.AddKeyedScoped<IOutboxQueue<string>>(_productsOutbox, (_, _) =>
        {
            return new OutboxQueue<string>(_productsOutbox, true);
        });

        services.AddScoped<IProductsAccessChecker, ProductsAccessChecker>();
        services.AddScoped<IProductsSearcher, ProductsSearcher>();
        services.AddScoped<IProductsObjectStorage>(provider =>
        {
            var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ProductsObjectStorage>>();
            var minioClient = provider.GetRequiredService<IMinioClient>();
            var outbox = provider.GetRequiredKeyedService<IOutboxQueue<string>>(_productsOutbox);
            var imagesBucket = configuration["ObjectStorage:Buckets:ProductsImages"]!;

            return new ProductsObjectStorage(logger, minioClient, outbox, imagesBucket);
        });

        services.AddScoped<IUsersService, UsersService>();

        services.AddHttpClient<IUsersService, UsersService>(cfg =>
        {
            var address = configuration["Services:Users:Address"]!;
            var timeout = int.Parse(configuration["Services:Users:Timeout"]!);

            cfg.BaseAddress = new Uri(address);
            cfg.Timeout = TimeSpan.FromMilliseconds(timeout);
        });

        return services;
    }

    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();

        db.Database.EnsureCreated();
    }
}
