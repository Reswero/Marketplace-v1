namespace Marketplace.Users.Application.Common.Interfaces;

/// <summary>
/// Паттерн Unit Of Work
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохранить изменения
    /// </summary>
    /// <param name="cancellationToken"></param>
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
