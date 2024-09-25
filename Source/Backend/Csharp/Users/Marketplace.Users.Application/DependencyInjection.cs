using FluentValidation;
using Marketplace.Users.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Marketplace.Users.Application;

/// <summary>
/// Инъекция зависимостей слоя приложения
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
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
