using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Command.CancelDelivery;

public record CancelDeliveryCommand(Guid DeliveryId)
    : IRequest<Delivery>;
