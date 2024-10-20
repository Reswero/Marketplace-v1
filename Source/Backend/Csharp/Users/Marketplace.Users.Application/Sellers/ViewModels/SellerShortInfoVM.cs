namespace Marketplace.Users.Application.Sellers.ViewModels;

/// <summary>
/// Краткая информация о продавце
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="CompanyName">Название компании</param>
public record SellerShortInfoVM(int AccountId, string CompanyName);
