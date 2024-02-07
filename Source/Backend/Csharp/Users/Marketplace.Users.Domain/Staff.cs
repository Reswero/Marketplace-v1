﻿namespace Marketplace.Users.Domain;

/// <summary>
/// Персонал
/// </summary>
public class Staff
{
    private Staff() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="firstName">Имя</param>
    /// <param name="lastName">Фамилия</param>
    public Staff(int accountId, string firstName, string lastName)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
    }

    public int AccountId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
}
