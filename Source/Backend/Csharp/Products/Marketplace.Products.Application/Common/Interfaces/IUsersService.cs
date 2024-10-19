using Marketplace.Products.Application.Users.Models;

namespace Marketplace.Products.Application.Common.Interfaces;

/// <summary>
/// Взаимодействие с сервисом пользователей
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Получить продавца
    /// </summary>
    /// <param name="id">Идентификатор аккаунта</param>
    /// <param name="cancellationToken"></param>
    public Task<Seller> GetSellerInfoAsync(int id, CancellationToken cancellationToken = default);
}
