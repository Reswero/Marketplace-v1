﻿using FluentValidation;
using Marketplace.Common.Responses;
using Marketplace.Common.SoftDelete.Exceptions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Domain.Exceptions;
using System.Net.Mime;

namespace Marketplace.Products.Api.Middlewares;

/// <summary>
/// Глобальный обработчик ошибок
/// </summary>
/// <param name="logger"></param>
/// <param name="next"></param>
public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e) when (e is ValidationException ||
                                  e is ObjectDeletedException ||
                                  e is ObjectAlreadyExistsException ||
                                  e is ImagesLimitExceededException)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status400BadRequest);
        }
        catch (ObjectNotFoundException e)
        {
            await HandleExceptionAsync(context, e, StatusCodes.Status404NotFound);
        }
        catch (UploadImagesException e)
        {
            _logger.LogError(e, "Ошибка при загрузке изображений. {Error}", e.Message);
            await HandleExceptionAsync(context, e, StatusCodes.Status500InternalServerError);
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
