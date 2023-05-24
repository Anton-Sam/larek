using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CompleteOrder;

public record CompleteOrderCommand(Guid OrderId)
    : IRequest<Order>;
