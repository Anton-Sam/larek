namespace Ordering.Application.Models;

public record Delivery(
    Guid Id,
    Guid OrderId,
    DateTime DeliveryDate);
