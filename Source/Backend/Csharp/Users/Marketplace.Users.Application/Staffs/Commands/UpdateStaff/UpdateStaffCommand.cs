using MediatR;

namespace Marketplace.Users.Application.Staffs.Commands.UpdateStaff;

/// <summary>
/// Модель обновления профиля персонала
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
public record UpdateStaffCommand(int AccountId, string FirstName, string LastName) : IRequest;
