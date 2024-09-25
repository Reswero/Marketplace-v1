using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Customers.Queries.GetCustomer;

/// <summary>
/// Модель получения профиля покупателя
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
public record GetCustomerQuery(int AccountId) : IRequest<Customer>;
