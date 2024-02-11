using Marketplace.Users.Domain;

namespace Marketplace.Users.Application.Common.Interfaces;

/// <summary>
/// Репозиторий персонала
/// </summary>
public interface IStaffsRepository
{
    /// <summary>
    /// Добавить персонал
    /// </summary>
    /// <param name="staff">Персонал</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(Staff staff, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить персонал
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="cancellationToken"></param>
    public Task<Staff> GetAsync(int accountId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить персонал
    /// </summary>
    /// <param name="staff">Персонал</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Staff staff, CancellationToken cancellationToken = default);
}
