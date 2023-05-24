using Delivering.Application.Features.Deliveries.Command.CancelDelivery;
using Delivering.Application.Features.Deliveries.Command.CompleteDelivery;
using Delivering.Application.Features.Deliveries.Command.CreateDelivery;
using Delivering.Application.Features.Deliveries.Command.StartProcessDelivery;
using Delivering.Application.Features.Deliveries.Queries.GetDeliveries;
using Delivering.Application.Features.Deliveries.Queries.GetDeliveryById;
using Delivering.WebApi.Dtos.Requests.Deliveries;
using Delivering.WebApi.Dtos.Responses.Deliveries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivering.WebApi.Controllers;

[Route("/api/delivering/deliveries")]
public class DeliveriesController : ApiController
{
    public DeliveriesController(ISender sender) : base(sender)
    { }

    [HttpPost]
    public async Task<IActionResult> CreateDelivery(
        CreateDeliveryRequest request,
        CancellationToken cancellationToken)
    {
        var delivery = await Sender.Send(
            new CreateDeliveryCommand(
                request.OrderId,
                request.DeliveryDate), cancellationToken);

        var response = new DeliveryResponse(
            delivery.Id,
            delivery.OrderId,
            delivery.CourierId,
            delivery.DeliveryDate,
            delivery.Status);

        return CreatedAtAction(
            nameof(GetDeliveryById),
            new { Id = delivery.Id },
            response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeliveryById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var delivery = await Sender.Send(
            new GetDeliveryByIdQuery(id),
            cancellationToken);

        var response = new DeliveryResponse(
            delivery.Id,
            delivery.OrderId,
            delivery.CourierId,
            delivery.DeliveryDate,
            delivery.Status);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetDeliveries(
        Guid courierId,
        CancellationToken cancellationToken)
    {
        var deliveries = await Sender.Send(
            new GetDeliveriesQuery(courierId),
            cancellationToken);

        var response = deliveries.Select(
            d => new DeliveryResponse(
                d.Id,
                d.OrderId,
                d.CourierId,
                d.DeliveryDate,
                d.Status));

        return Ok(response);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelDelivery(
        Guid id,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new CancelDeliveryCommand(id),
            cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/process")]
    public async Task<IActionResult> StartProcessDelivery(
        Guid id,
        Guid courierId,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new StartProcessDeliveryCommand(id, courierId),
            cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> CompleteDelivery(
        Guid id,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new CompleteDeliveryCommand(id),
            cancellationToken);

        return Ok();
    }
}
