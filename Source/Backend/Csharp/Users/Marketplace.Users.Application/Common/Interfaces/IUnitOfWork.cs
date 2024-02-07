namespace Marketplace.Users.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
