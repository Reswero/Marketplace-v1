using Marketplace.Users.Application.Common.Exceptions;
using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Users.Infrastructure.Administrators.Persistence;

/// <summary>
/// Репозиторий администраторов
/// </summary>
internal class AdministratorsRepository(MarketplaceContext db) : IAdministratorsRepository
{
    private readonly MarketplaceContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(Administrator admin, CancellationToken cancellationToken = default)
    {
        await _db.Administrators.AddAsync(admin, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Administrator> GetAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var admin = await _db.Administrators.FirstOrDefaultAsync(a => a.AccountId == accountId, cancellationToken);
        return admin is null ? throw new AccountNotExistsException(accountId) : admin;
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Administrator admin, CancellationToken cancellationToken = default)
    {
        _db.Administrators.Update(admin);
        return Task.CompletedTask;
    }
}
