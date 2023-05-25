using Ordering.Domain.OrderAggregate;
using System.Text.Json.Serialization;

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
    [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderStatus Status,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] DeliveryType DeliveryType,
    DateTime? DeliveryDate,
    IEnumerable<OrderItemResponse> Items);
