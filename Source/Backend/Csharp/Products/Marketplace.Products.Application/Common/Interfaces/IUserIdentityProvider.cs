namespace Marketplace.Products.Application.Common.Interfaces;

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
