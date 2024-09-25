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
    public const string TypeHeader = "X-Account-Header";
    /// <summary>
    /// Ключ авторизационных данных для <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/>
    /// </summary>
    public const string ClaimsKey = "AuthClaims";
}
