using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Staffs.Queries.GetStaff;

/// <summary>
/// Модель получения профиля персонала
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
public record GetStaffQuery(int AccountId) : IRequest<Staff>;
