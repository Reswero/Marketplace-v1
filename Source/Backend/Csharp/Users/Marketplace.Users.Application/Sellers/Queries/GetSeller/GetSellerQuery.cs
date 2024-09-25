using Marketplace.Users.Domain;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Queries.GetSeller;

/// <summary>
/// Модель получения профиля продавца
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
public record GetSellerQuery(int AccountId) : IRequest<Seller>;
