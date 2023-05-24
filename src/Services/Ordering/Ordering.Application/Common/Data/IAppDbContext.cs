using Microsoft.EntityFrameworkCore;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Common.Data;

public interface IAppDbContext
{
    DbSet<Order> Orders { get; set; }
    DbSet<Buyer> Buyers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
