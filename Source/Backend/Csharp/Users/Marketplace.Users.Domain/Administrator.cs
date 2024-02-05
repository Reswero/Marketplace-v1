﻿namespace Marketplace.Users.Domain;

/// <summary>
/// Администратор
/// </summary>
/// <param name="accountId">Идентификатор аккаунта</param>
/// <param name="firstName">Имя</param>
/// <param name="lastName">Фамилия</param>
public class Administrator(int accountId, string firstName, string lastName)
{
    public int AccountId { get; private set; } = accountId;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
}
