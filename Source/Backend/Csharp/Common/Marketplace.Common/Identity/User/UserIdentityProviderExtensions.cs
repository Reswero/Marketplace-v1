using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Identity.User;

/// <summary>
/// Расширения для провайдера идентификационных данных пользователя
/// </summary>
public static class UserIdentityProviderExtensions
{
    /// <summary>
    /// Добавить провайдер идентификационных данных пользователя из HttpContext'а
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpUserIdentityProvider(this IServiceCollection services)
        => services.AddScoped<IUserIdentityProvider, HttpUserIdentityProvider>();
}
