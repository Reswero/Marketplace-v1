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
    
    /// <summary>
    /// Проверка доступа к аккаунта по идентификатору
    /// </summary>
    /// <param name="context"></param>
    /// <param name="id">Идентификатор</param>
    public static bool CheckAccessById(this HttpContext context, int id)
    {
        var claims = context.GetClaims();

        if (claims.Id == id)
            return true;

        if (claims.Type == AccountType.Admin)
            return true;

        return false;
    }
}
