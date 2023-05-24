using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId)
    : IRequest<Order>;
