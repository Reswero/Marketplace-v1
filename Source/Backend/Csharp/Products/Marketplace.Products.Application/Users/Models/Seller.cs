namespace Marketplace.Products.Application.Users.Models;

/// <summary>
/// Продавец
/// </summary>
/// <param name="AccountId">Идентификатор аккаунта</param>
/// <param name="CompanyName">Название компании</param>
public record Seller(int AccountId, string CompanyName);
