using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid BuyerId,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    DeliveryType DeliveryType,
    DateTime? DeliveryDate) : IRequest<Order>;
