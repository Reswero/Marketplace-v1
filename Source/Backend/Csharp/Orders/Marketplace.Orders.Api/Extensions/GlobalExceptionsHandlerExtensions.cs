using Marketplace.Orders.Api.Middlewares;

namespace Marketplace.Orders.Api.Extensions;

/// <summary>
/// Расширения для глобального обработчика ошибок
/// </summary>
public static class GlobalExceptionsHandlerExtensions
{
    /// <summary>
    /// Использовать глобальный обработчик ошибок
    /// </summary>
    /// <param name="app"></param>
    public static IApplicationBuilder UseGlobalExceptionsHandler(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionsHandlerMiddleware>();
}
