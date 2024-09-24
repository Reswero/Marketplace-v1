using MediatR;

namespace Marketplace.Users.Application.Sellers.Commands.UpdateSeller;

/// <summary>
/// Модель обновления профиля продавца
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
/// <param name="CompanyName">Название компании</param>
public record UpdateSellerCommand(int AccountId, string FirstName, string LastName,
    string CompanyName) : IRequest;
