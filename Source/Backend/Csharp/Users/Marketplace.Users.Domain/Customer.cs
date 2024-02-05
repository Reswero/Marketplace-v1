namespace Marketplace.Users.Domain;

/// <summary>
/// Покупатель
/// </summary>
/// <param name="accountId">Идентификатор аккаунта</param>
/// <param name="firstName">Имя</param>
/// <param name="lastName">Фамилия</param>
public class Customer(int accountId, string firstName, string lastName)
{
    private readonly List<Address> _addresses = [];

    /// <summary>
    /// Идентификатор аккаунта
    /// </summary>
    public int AccountId { get; private set; } = accountId;
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; private set; } = firstName;
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; private set; } = lastName;
    /// <summary>
    /// Адреса
    /// </summary>
    public IReadOnlyCollection<Address> Addresses => _addresses;
}
