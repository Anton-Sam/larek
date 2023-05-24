using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Command.CompleteDelivery;

public record CompleteDeliveryCommand(Guid DeliveryId)
    : IRequest<Delivery>;
