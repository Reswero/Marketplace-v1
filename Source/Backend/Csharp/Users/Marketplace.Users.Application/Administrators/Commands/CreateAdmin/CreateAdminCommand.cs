using MediatR;

namespace Marketplace.Users.Application.Administrators.Commands.CreateAdmin;

/// <summary>
/// Модель создания профиля администратора
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
public record CreateAdminCommand(int AccountId, string FirstName, string LastName) : IRequest;
