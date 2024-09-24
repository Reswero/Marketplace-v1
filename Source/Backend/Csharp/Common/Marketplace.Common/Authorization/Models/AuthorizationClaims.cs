namespace Marketplace.Common.Authorization.Models;

/// <summary>
/// Авторизационные данные
/// </summary>
/// <param name="id">Идентификатор аккаунта</param>
/// <param name="type">Тип аккаунта</param>
public readonly struct AuthorizationClaims(int id, AccountType type)
{
    /// <summary>
    /// Идентификатор аккаунта
    /// </summary>
    public readonly int Id = id;
    /// <summary>
    /// Тип аккаунта
    /// </summary>
    public readonly AccountType Type = type;
}
