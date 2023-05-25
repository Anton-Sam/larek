using Ordering.Application.Models;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Common.Interfaces;

public interface IDeliveryService
{
    Task<Delivery?> CreateDeliveryAsync(
        Order order,
        CancellationToken cancellationToken = default);
}
