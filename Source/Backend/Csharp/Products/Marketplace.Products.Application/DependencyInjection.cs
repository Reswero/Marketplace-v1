using FluentValidation;
using Marketplace.Common.Mediator.Behaviors;
using Marketplace.Products.Application.Products.ViewModels.Validators;
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
        Type[] excludedValidators =
        [
            typeof(AddProductParameterVMValidator),
            typeof(UpdateProductParameterVMValidator)
        ];
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(),
            filter: f => !excludedValidators.Contains(f.ValidatorType), includeInternalTypes: true);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
