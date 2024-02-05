namespace Marketplace.Users.Domain;

/// <summary>
/// Адрес покупателя
/// </summary>
/// <param name="accountId">Идентификатор аккаунта</param>
/// <param name="location">Местоположение</param>
public class Address(int accountId, string location)
{
    /// <summary>
    /// Идентификатор аккаунта
    /// </summary>
    public int AccountId { get; set; } = accountId;
    /// <summary>
    /// Местоположение
    /// </summary>
    public string Location { get; set; } = location;
}
