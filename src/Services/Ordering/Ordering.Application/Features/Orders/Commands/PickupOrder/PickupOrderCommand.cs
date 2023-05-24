using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.PickupOrder;

public record PickupOrderCommand(Guid OrderId)
    : IRequest<Order>;
