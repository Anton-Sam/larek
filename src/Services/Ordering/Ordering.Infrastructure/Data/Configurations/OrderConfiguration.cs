using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.OwnsOne(o => o.Address);

        builder.OwnsMany(o => o.Items, ib =>
        {
            ib.ToTable("OrderItems");

            ib.WithOwner().HasForeignKey("OrderId");

            ib.HasKey("Id", "OrderId");
        });
    }
}
