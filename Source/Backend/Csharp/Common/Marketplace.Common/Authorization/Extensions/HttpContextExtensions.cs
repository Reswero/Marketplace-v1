using Marketplace.Common.Authorization.Models;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.Authorization.Extensions;

/// <summary>
/// Расширения HttpContext для получения авторизационных данных
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Получение авторизационных данных
    /// </summary>
    /// <param name="context"></param>
    public static AuthorizationClaims GetClaims(this HttpContext context)
        => context.Items.TryGetValue(AuthorizationConsts.ClaimsKey, out var claims) ?
            (AuthorizationClaims) claims : new AuthorizationClaims();
}
