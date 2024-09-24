using MediatR;

namespace Marketplace.Users.Application.Customers.Commands.UpdateCustomer;

/// <summary>
/// Модель обновления профиля покупателя
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
public record UpdateCustomerCommand(int AccountId, string FirstName, string LastName) : IRequest;
