using Marketplace.Users.Application.Common.Exceptions;
using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using Marketplace.Users.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Users.Infrastructure.Customers.Persistence;

/// <summary>
/// Репозиторий покупателей
/// </summary>
internal class CustomersRepository(MarketplaceContext db) : ICustomersRepository
{
    private readonly MarketplaceContext _db = db;

    /// <inheritdoc/>
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _db.Customers.AddAsync(customer, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Customer> GetAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.AccountId == accountId, cancellationToken);
        return customer is null ? throw new AccountNotExistsException(accountId) : customer;
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _db.Customers.Update(customer);
        return Task.CompletedTask;
    }
}
