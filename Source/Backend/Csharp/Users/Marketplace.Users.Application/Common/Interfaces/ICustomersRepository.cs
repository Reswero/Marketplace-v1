using Marketplace.Users.Domain;

namespace Marketplace.Users.Application.Common.Interfaces;

/// <summary>
/// Репозиторий покупателей
/// </summary>
public interface ICustomersRepository
{
    /// <summary>
    /// Добавить покупателя
    /// </summary>
    /// <param name="customer">Покупатель</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить покупателя
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="cancellationToken"></param>
    public Task<Customer> GetAsync(int accountId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить покупателя
    /// </summary>
    /// <param name="customer">Покупатель</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
}
