using Marketplace.Users.Application.Sellers.ViewModels;
using MediatR;

namespace Marketplace.Users.Application.Sellers.Queries.GetSellerShortInfo;

/// <summary>
/// Модель запроса краткой информации о продавце
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
public record GetSellerShortInfoQuery(int AccountId)
    : IRequest<SellerShortInfoVM>;
