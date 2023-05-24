using Delivering.Domain.DeliveryAggregate;

namespace Delivering.WebApi.Dtos.Responses.Deliveries;

public record DeliveryResponse(
    Guid Id,
    Guid OrderId,
    Guid? CourierId,
    DateTime DeliveryDate,
    DeliveryStatus Status);
