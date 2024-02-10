using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Marketplace.Users.Infrastructure.Customers.Persistence;
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

        services.AddScoped<ICustomersRepository, CustomersRepository>();

        return services;
    }
}
