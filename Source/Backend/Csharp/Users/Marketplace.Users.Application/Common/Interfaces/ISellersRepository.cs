using Marketplace.Users.Domain;

namespace Marketplace.Users.Application.Common.Interfaces;

/// <summary>
/// Репозиторий продавцов
/// </summary>
public interface ISellersRepository
{
    /// <summary>
    /// Добавить продавца
    /// </summary>
    /// <param name="seller">Продавец</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(Seller seller, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить продавца
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="cancellationToken"></param>
    public Task<Seller> GetAsync(int accountId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить продавца
    /// </summary>
    /// <param name="seller">Продавец</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Seller seller, CancellationToken cancellationToken = default);
}
