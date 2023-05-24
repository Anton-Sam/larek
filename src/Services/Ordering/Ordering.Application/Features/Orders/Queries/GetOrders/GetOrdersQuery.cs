using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Queries.GetOrders;

public record GetOrdersQuery(Guid BuyerId)
    : IRequest<IEnumerable<Order>>;
