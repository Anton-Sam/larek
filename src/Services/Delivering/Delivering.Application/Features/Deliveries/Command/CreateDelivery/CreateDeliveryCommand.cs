using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Command.CreateDelivery;

public record CreateDeliveryCommand(
    Guid OrderId,
    DateTime DeliveryDate) : IRequest<Delivery>;
