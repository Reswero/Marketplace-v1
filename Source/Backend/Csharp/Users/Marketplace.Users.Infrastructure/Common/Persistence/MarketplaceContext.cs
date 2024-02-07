using Marketplace.Users.Application.Common.Interfaces;
using Marketplace.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Marketplace.Users.Infrastructure.Common.Persistence;

internal class MarketplaceContext(DbContextOptions<MarketplaceContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Seller> Sellers { get; set; } = null!;
    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Administrator> Administrators { get; set; } = null!;
    public DbSet<Address> Address { get; set; } = null!;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
