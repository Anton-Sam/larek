using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Command.StartProcessDelivery;

public record StartProcessDeliveryCommand(Guid DeliveryId, Guid CourierId)
    : IRequest<Delivery>;
