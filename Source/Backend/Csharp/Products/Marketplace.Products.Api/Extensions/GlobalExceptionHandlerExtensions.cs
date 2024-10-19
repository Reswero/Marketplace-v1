using Marketplace.Products.Api.Middlewares;

namespace Marketplace.Products.Api.Extensions;

/// <summary>
/// Расширение для обработки ошибок
/// </summary>
public static class GlobalExceptionHandlerExtensions
{
    /// <summary>
    /// Использовать глобальный обработчик ошибок
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
