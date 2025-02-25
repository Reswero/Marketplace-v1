namespace Marketplace.Common.Identity.User;

/// <summary>
/// Провайдер идентификационных данных пользователя
/// </summary>
public interface IUserIdentityProvider
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int? Id { get; }
}
