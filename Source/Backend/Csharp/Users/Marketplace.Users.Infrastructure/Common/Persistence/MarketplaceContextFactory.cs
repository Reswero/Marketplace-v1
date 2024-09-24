using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Marketplace.Users.Infrastructure.Common.Persistence;

internal class MarketplaceContextFactory : IDesignTimeDbContextFactory<MarketplaceContext>
{
    public MarketplaceContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<MarketplaceContext> builder = new();
        builder.UseNpgsql();

        return new MarketplaceContext(builder.Options);
    }
}
