using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.AddOrderItems;

public record AddOrderItemsCommand(
    Guid OrderId,
    Guid ItemId,
    decimal Price,
    uint Count) : IRequest<Order>;
