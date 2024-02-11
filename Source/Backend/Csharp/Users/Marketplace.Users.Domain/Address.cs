namespace Marketplace.Users.Domain;

/// <summary>
/// Адрес покупателя
/// </summary>
public class Address
{
    private Address() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="location">Местоположение</param>
    public Address(int accountId, string location)
    {
        AccountId = accountId;
        Location = location;
    }

    /// <summary>
    /// Идентификатор аккаунта
    /// </summary>
    public int AccountId { get; private set; }
    /// <summary>
    /// Местоположение
    /// </summary>
    public string Location { get; private set; } = null!;
}
