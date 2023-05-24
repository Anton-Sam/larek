using Ordering.Domain.OrderAggregate;

namespace Ordering.WebApi.Dtos.Responses.Orders;

public record OrderResponse(
    Guid Id,
    Guid BuyerId,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    uint TotalCount,
    decimal TotalPrice,
    OrderStatus Status,
    DeliveryType DeliveryType,
    IEnumerable<OrderItemResponse> Items);
