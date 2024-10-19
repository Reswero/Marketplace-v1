namespace Marketplace.Common.Authorization.Models;

/// <summary>
/// Тип аккаунта
/// </summary>
[Flags]
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
    Staff = 4,
    /// <summary>
    /// Администратор
    /// </summary>
    Admin = 8
}
