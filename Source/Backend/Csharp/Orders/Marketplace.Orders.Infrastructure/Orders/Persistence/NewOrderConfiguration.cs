using Marketplace.Orders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Orders.Infrastructure.Orders.Persistence;

internal class NewOrderConfiguration : IEntityTypeConfiguration<NewOrder>
{
    public void Configure(EntityTypeBuilder<NewOrder> builder)
    {
        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.HasOne(o => o.Order)
            .WithMany()
            .HasForeignKey(o => o.Id);
    }
}
