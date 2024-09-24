using MediatR;

namespace Marketplace.Users.Application.Staffs.Commands.CreateStaff;

/// <summary>
/// Модель создания профиля персонала
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
public record CreateStaffCommand(int AccountId, string FirstName, string LastName) : IRequest;
