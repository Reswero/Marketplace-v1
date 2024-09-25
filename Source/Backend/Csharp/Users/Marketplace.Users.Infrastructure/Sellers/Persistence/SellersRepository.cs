using Marketplace.Users.Application.Common.Exceptions;
using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Users.Infrastructure.Sellers.Persistence;

/// <summary>
/// Репозиторий продавцов
/// </summary>
/// <param name="db">Контекст БД</param>
internal class SellersRepository(MarketplaceContext db) : ISellersRepository
{
    private readonly MarketplaceContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(Seller seller, CancellationToken cancellationToken = default)
    {
        await _db.Sellers.AddAsync(seller, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Seller> GetAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var seller = await _db.Sellers.FirstOrDefaultAsync(s => s.AccountId == accountId, cancellationToken);
        return seller is null ? throw new AccountNotExistsException(accountId) : seller;
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Seller seller, CancellationToken cancellationToken = default)
    {
        _db.Sellers.Update(seller);
        return Task.CompletedTask;
    }
}
