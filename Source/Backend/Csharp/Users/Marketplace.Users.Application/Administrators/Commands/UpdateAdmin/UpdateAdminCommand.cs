using MediatR;

namespace Marketplace.Users.Application.Administrators.Commands.UpdateAdmin;

/// <summary>
/// Модель обновления профиля администратора
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
public record UpdateAdminCommand(int AccountId, string FirstName, string LastName) : IRequest;
