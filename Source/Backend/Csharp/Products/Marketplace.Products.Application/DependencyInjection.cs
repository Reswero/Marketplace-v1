using FluentValidation;
using Marketplace.Common.Mediator.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Marketplace.Products.Application;

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
        ValidatorOptions.Global.LanguageManager.Culture = new("ru-RU");

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
