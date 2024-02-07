using Marketplace.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Users.Infrastructure.Sellers.Persistence;

internal class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.HasKey(s => s.AccountId);
        builder.HasIndex(s => s.CompanyName).IsUnique();

        builder.Property(s => s.AccountId).ValueGeneratedNever();

        builder.Property(s => s.FirstName).HasMaxLength(20);
        builder.Property(s => s.LastName).HasMaxLength(40);
        builder.Property(s => s.CompanyName).HasMaxLength(100);
    }
}
