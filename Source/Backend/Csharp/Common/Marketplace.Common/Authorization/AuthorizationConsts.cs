using Marketplace.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Marketplace.Common.Authorization;

/// <summary>
/// Константы авторизации
/// </summary>
public static class AuthorizationConsts
{
    /// <summary>
    /// Название заголовка с идентификатором аккаунта
    /// </summary>
    public const string IdHeader = "X-Account-Id";
    /// <summary>
    /// Название заголовка с типом аккаунта
    /// </summary>
    public const string TypeHeader = "X-Account-Type";
    /// <summary>
    /// Ключ авторизационных данных для <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/>
    /// </summary>
    public const string ClaimsKey = "AuthClaims";

    /// <summary>
    /// Ответ "Не авторизован"
    /// </summary>
    public static readonly JsonResult UnauthorizedResultResponse = new("Unauthorized")
    {
        ContentType = MediaTypeNames.Application.Json,
        StatusCode = StatusCodes.Status401Unauthorized,
        Value = new ErrorResponse(StatusCodes.Status401Unauthorized, "Не авторизован")
    };
    /// <summary>
    /// Ответ "Доступ запрещен"
    /// </summary>
    public static readonly JsonResult ForbidResultResponse = new("Forbid")
    {
        ContentType = MediaTypeNames.Application.Json,
        StatusCode = StatusCodes.Status403Forbidden,
        Value = new ErrorResponse(StatusCodes.Status403Forbidden, "Доступ запрещен")
    };
}
