using Marketplace.Common.Transactions;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;
using Marketplace.Products.Domain.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Marketplace.Products.Infrastructure.Common.Persistence;

internal class ProductsContext(DbContextOptions<ProductsContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Subсategory> Subcategories { get; set; } = null!;
    public DbSet<CategoryParameter> CategoryParameters { get; set; } = null!;
    public DbSet<ProductParameter> ProductParameters { get; set; } = null!;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
