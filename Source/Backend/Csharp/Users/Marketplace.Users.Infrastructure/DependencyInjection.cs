using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Infrastructure.Administrators.Persistence;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Marketplace.Users.Infrastructure.Customers.Persistence;
using Marketplace.Users.Infrastructure.Sellers.Persistence;
using Marketplace.Users.Infrastructure.Staffs.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Marketplace.Users.Infrastructure;

/// <summary>
/// Иъекция зависимостей слоя инфраструктуры
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить слой инфраструктуры
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
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

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MarketplaceContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<MarketplaceContext>());
        services.AddScoped<ICustomersRepository, CustomersRepository>();
        services.AddScoped<ISellersRepository, SellersRepository>();
        services.AddScoped<IStaffsRepository, StaffsRepository>();
        services.AddScoped<IAdministratorsRepository, AdministratorsRepository>();

        return services;
    }

    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<MarketplaceContext>();

        db.Database.Migrate();
    }
}
