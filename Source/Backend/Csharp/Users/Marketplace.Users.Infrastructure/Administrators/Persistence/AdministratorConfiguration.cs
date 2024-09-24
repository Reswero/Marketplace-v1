using Marketplace.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Users.Infrastructure.Administrators.Persistence;

internal class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.HasKey(a => a.AccountId);

        builder.HasIndex(a => a.AccountId).IsUnique();

        builder.Property(a => a.AccountId).ValueGeneratedNever();

        builder.Property(a => a.FirstName).HasMaxLength(20);
        builder.Property(a => a.LastName).HasMaxLength(40);
    }
}
