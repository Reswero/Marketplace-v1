using Marketplace.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Users.Infrastructure.Customers.Persistence;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.AccountId);

        builder.HasIndex(c => c.AccountId).IsUnique();

        builder.Property(c => c.AccountId).ValueGeneratedNever();

        builder.Property(c => c.FirstName).HasMaxLength(20);
        builder.Property(c => c.LastName).HasMaxLength(40);
    }
}
