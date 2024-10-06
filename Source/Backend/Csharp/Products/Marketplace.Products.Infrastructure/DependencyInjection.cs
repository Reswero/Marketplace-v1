﻿using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Infrastructure.Categories.Persistence;
using Marketplace.Products.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContextFactory<ProductsContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddScoped<ICategoriesRepository, CategoriesRepository>();

        return services;
    }

    public static void InitializeDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();

        db.Database.EnsureCreated();
    }
}