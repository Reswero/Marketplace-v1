namespace Marketplace.Products.Application.Users.ViewModels;

/// <summary>
/// Модель продавца
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="CompanyName">Название компании</param>
public record SellerVM(int AccountId, string CompanyName);
