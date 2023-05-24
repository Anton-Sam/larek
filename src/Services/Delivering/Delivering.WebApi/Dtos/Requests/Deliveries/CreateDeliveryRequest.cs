namespace Delivering.WebApi.Dtos.Requests.Deliveries;

public record CreateDeliveryRequest(
    Guid OrderId,
    DateTime DeliveryDate);
