using Delivering.Domain.DeliveryAggregate;
using System.Text.Json.Serialization;

namespace Delivering.WebApi.Dtos.Responses.Deliveries;

public record DeliveryResponse(
    Guid Id,
    Guid OrderId,
    Guid? CourierId,
    DateTime DeliveryDate,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] DeliveryStatus Status);
