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
using Serilog;

namespace Marketplace.Products.Infrastructure;

/// <summary>
/// Инъекция зависимостей слоя инфраструктуры
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
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ProductsContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
            opt.AddInterceptors(new SoftDeleteInterceptor());
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ProductsContext>());
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        services.AddScoped<IProductsAccessChecker, ProductsAccessChecker>();
        services.AddScoped<IProductsSearcher, ProductsSearcher>();

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
