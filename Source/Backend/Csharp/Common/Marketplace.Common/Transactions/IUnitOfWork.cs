namespace Marketplace.Common.Transactions;

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
