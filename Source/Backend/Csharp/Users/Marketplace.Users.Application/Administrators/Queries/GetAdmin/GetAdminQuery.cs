using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Administrators.Queries.GetAdmin;

/// <summary>
/// Модель получения профиля администратора
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
public record GetAdminQuery(int AccountId) : IRequest<Administrator>;
