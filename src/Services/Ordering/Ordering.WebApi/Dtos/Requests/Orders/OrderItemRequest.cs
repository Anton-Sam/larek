namespace Ordering.WebApi.Dtos.Requests.Orders;

public record OrderItemRequest(
    Guid ItemId,
    decimal Price,
    uint Count);
