using Ordering.Domain.OrderAggregate;

namespace Ordering.WebApi.Dtos.Requests.Orders;

public record CreateOrderRequest(
    Guid BuyerId,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    DeliveryType DeliveryType);
