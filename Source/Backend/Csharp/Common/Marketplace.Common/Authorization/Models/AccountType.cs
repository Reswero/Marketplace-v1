namespace Marketplace.Common.Authorization.Models;

/// <summary>
/// Тип аккаунта
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Отсутствует
    /// </summary>
    None = 0,
    /// <summary>
    /// Покупатель
    /// </summary>
    Customer = 1,
    /// <summary>
    /// Продавец
    /// </summary>
    Seller = 2,
    /// <summary>
    /// Персонал
    /// </summary>
    Staff = 3,
    /// <summary>
    /// Администратор
    /// </summary>
    Admin = 4
}
