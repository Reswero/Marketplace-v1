using Marketplace.Common.Authorization.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Marketplace.Common.Authorization.Extensions;

/// <summary>
/// Расширения для авторизации
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Использовать авторизационные данные из заголовков запроса
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHeaderAuthorization(this IApplicationBuilder builder)
        => builder.UseMiddleware<AuthorizationMiddleware>();
}
