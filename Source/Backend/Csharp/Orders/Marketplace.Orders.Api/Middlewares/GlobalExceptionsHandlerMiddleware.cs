using Marketplace.Common.Responses;
using Marketplace.Orders.Application.Common.Exceptions;
using System.Net.Mime;

namespace Marketplace.Orders.Api.Middlewares;

/// <summary>
/// Глобальный обработчик ошибок
/// </summary>
/// <param name="logger"></param>
/// <param name="next"></param>
public class GlobalExceptionsHandlerMiddleware(ILogger<GlobalExceptionsHandlerMiddleware> logger, RequestDelegate next)
{
    private readonly ILogger<GlobalExceptionsHandlerMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Вызов
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e) when (e is ImpossibleToPayOrderException ||
                                  e is ProductNotAvailableException)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status400BadRequest);
        }
        catch (AccessDeniedException e)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status403Forbidden);
        }
        catch (Exception e) when (e is OrderNotFoundException ||
                                  e is ProductNotFoundException)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status404NotFound);
        }
        catch (NotImplementedException e)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status501NotImplemented);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время выполнения запроса. {Error}", e.Message);
            await HandleExceptionAsync(context, new Exception("Неизвестная ошибка"), StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception e, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        await context.Response.WriteAsJsonAsync(new ErrorResponse(statusCode, e.Message));
    }
}
