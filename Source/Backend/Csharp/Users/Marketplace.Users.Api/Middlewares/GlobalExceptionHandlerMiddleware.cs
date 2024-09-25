using FluentValidation;
using Marketplace.Common.Responses;
using Marketplace.Users.Application.Common.Exceptions;
using System.Net.Mime;

namespace Marketplace.Users.Api.Middlewares;

/// <summary>
/// Глобальный обработчик ошибок
/// </summary>
/// <param name="logger"></param>
/// <param name="next"></param>
public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (ValidationException e)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status400BadRequest);
        }
        catch (AccountNotExistsException e)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status404NotFound);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время выполнения запроса: {Error}", e.Message);
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
