using MediatR;

namespace Marketplace.Users.Application.Sellers.Commands.CreateSeller;

/// <summary>
/// Модель создания профиля продавца
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
/// <param name="CompanyName">Название компании</param>
public record CreateSellerCommand(int AccountId, string FirstName, string LastName,
    string CompanyName) : IRequest;
