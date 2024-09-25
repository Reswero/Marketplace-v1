using MediatR;

namespace Marketplace.Users.Application.Customers.Commands.CreateCustomer;

/// <summary>
/// Модель создания профиля покупателя
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
public record CreateCustomerCommand(int AccountId, string FirstName, string LastName) : IRequest;
