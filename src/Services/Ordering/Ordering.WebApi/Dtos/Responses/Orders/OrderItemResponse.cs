namespace Ordering.WebApi.Dtos.Responses.Orders;

public record OrderItemResponse(
    Guid ItemId,
    decimal Price,
    uint Count);
