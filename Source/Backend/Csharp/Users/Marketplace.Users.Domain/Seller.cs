namespace Marketplace.Users.Domain;

/// <summary>
/// Продавец
/// </summary>
public class Seller
{
    private Seller() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="firstName">Имя</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="companyName">Название компании</param>
    public Seller(int accountId, string firstName, string lastName, string companyName)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        CompanyName = companyName;
    }

    /// <summary>
    /// Идентификатор компании
    /// </summary>
    public int AccountId { get; private set; }
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; private set; } = null!;
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; private set; } = null!;
    /// <summary>
    /// Название компании
    /// </summary>
    public string CompanyName { get; private set; } = null!;
}
