using Marketplace.Users.Application.Common.Exceptions;
using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Users.Infrastructure.Staffs.Persistence;

/// <summary>
/// Репозиторий персонала
/// </summary>
/// <param name="db">Контекст БД</param>
internal class StaffsRepository(MarketplaceContext db) : IStaffsRepository
{
    private readonly MarketplaceContext _db = db;

    public async Task AddAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        await _db.Staffs.AddAsync(staff, cancellationToken);
    }

    public async Task<Staff> GetAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var staff = await _db.Staffs.FirstOrDefaultAsync(s => s.AccountId == accountId, cancellationToken);
        return staff is null ? throw new AccountNotExistsException(accountId) : staff;
    }

    public Task UpdateAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        _db.Staffs.Update(staff);
        return Task.CompletedTask;
    }
}
