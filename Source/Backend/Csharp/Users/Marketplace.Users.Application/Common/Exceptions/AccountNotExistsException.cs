namespace Marketplace.Users.Application.Common.Exceptions;

/// <summary>
/// Несуществующий аккаунт
/// </summary>
/// <param name="id">Идентификатор аккаунта</param>
public class AccountNotExistsException(int id) : Exception($"Аккаунт с идентификатором {id} не существует")
{
    /// <summary>
    /// Идентификатор аккаунта
    /// </summary>
    public readonly int Id = id;
}
