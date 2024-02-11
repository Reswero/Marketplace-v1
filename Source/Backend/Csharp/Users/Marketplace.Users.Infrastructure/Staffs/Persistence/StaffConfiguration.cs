using Marketplace.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Users.Infrastructure.Staffs.Persistence;

internal class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(s => s.AccountId);

        builder.Property(s => s.AccountId).ValueGeneratedNever();

        builder.Property(s => s.FirstName).HasMaxLength(20);
        builder.Property(s => s.LastName).HasMaxLength(40);
    }
}
