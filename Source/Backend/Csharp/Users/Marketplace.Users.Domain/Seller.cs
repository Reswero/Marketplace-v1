namespace Marketplace.Users.Domain;

/// <summary>
/// Покупатель
/// </summary>
/// <param name="accountId">Идентификатор аккаунта</param>
/// <param name="firstName">Имя</param>
/// <param name="lastName">Фамилия</param>
/// <param name="companyName">Название компании</param>
public class Seller(int accountId, string firstName, string lastName, string companyName)
{
    /// <summary>
    /// Идентификатор компании
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
    /// Название компании
    /// </summary>
    public string CompanyName { get; private set; } = companyName;
}
