namespace Ordering.WebApi.Dtos.Requests.Orders;

public record CreateOrderRequest(
    Guid BuyerId,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    string DeliveryType,
    DateTime? DeliveryDate);
