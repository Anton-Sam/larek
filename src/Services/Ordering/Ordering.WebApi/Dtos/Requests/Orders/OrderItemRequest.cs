namespace Ordering.WebApi.Dtos.Requests.Orders;

public record OrderItemRequest(
    Guid ItemId,
    uint Count);
