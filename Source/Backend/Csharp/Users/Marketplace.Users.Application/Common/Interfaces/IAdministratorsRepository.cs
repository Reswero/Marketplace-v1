using Marketplace.Users.Domain;

namespace Marketplace.Users.Application.Common.Interfaces;

/// <summary>
/// Репозиторий администраторов
/// </summary>
public interface IAdministratorsRepository
{
    /// <summary>
    /// Добавить администратора
    /// </summary>
    /// <param name="admin">Администратор</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(Administrator admin, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить администратора
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="cancellationToken"></param>
    public Task<Administrator> GetAsync(int accountId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить администратора
    /// </summary>
    /// <param name="admin">Администратор</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Administrator admin, CancellationToken cancellationToken = default);
}
