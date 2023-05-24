using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Queries.GetDeliveries;

public record class GetDeliveriesQuery(Guid CourierId)
    : IRequest<IEnumerable<Delivery>>;

