using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Marketplace.Orders.Application;

/// <summary>
/// Внедрение зависимостей слоя приложения
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить слой приложения
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
