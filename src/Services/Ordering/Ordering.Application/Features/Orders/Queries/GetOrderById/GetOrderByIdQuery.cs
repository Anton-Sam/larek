using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId)
    : IRequest<Order>;
