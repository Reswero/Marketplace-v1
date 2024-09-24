using Marketplace.Common.Authorization.Models;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.Authorization.Middlewares;

/// <summary>
/// Получает авторизационные данные из заголовков
/// </summary>
/// <param name="next"></param>
public class AuthorizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var idExists = context.Request.Headers.TryGetValue(AuthorizationConsts.IdHeader, out var idString);
        var typeExists = context.Request.Headers.TryGetValue(AuthorizationConsts.TypeHeader, out var typeString);

        int id = 0;
        AccountType type = AccountType.None;
        if (idExists && typeExists)
        {
            _ = int.TryParse(idString, out id);
            _ = Enum.TryParse(typeString, out type);
        }

        context.Items[AuthorizationConsts.ClaimsKey] = new AuthorizationClaims(id, type);

        await _next(context);
    }
}
