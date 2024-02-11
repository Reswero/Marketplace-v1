using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Marketplace.Users.Infrastructure.Customers.Persistence;
using Marketplace.Users.Infrastructure.Sellers.Persistence;
using Marketplace.Users.Infrastructure.Staffs.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MarketplaceContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<MarketplaceContext>());
        services.AddScoped<ICustomersRepository, CustomersRepository>();
        services.AddScoped<ISellersRepository, SellersRepository>();
        services.AddScoped<IStaffsRepository, StaffsRepository>();

        return services;
    }

    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<MarketplaceContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}
