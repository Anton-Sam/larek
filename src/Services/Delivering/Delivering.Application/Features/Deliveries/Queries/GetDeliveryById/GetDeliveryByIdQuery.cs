using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Queries.GetDeliveryById;

public record GetDeliveryByIdQuery(Guid DeliveryId)
    : IRequest<Delivery>;
