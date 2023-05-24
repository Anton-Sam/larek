using Delivering.Domain.DeliveryAggregate;
using Delivering.Domain.СourierAggregate;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Common.Data;

public interface IAppDbContext
{
    DbSet<Courier> Couriers { get; set; }
    DbSet<Delivery> Deliveries { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
