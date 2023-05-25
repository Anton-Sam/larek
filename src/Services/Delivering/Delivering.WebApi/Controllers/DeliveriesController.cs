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
[Produces("application/json")]
public class DeliveriesController : ApiController
{
    public DeliveriesController(ISender sender) : base(sender)
    { }

    /// <summary>
    /// Create new deliver.
    /// </summary>
    /// <remarks>
    /// Delivery requires order. Courier may take delivery later. It's called when order with delivery created.
    /// </remarks>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>New created delivery</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /deliveries
    ///     {
    ///         "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///           "deliveryDate": "2023-05-25T11:24:57.154Z"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created delivery</response>
    [HttpPost]
    [ProducesResponseType(typeof(DeliveryResponse), StatusCodes.Status201Created)]
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

    /// <summary>
    /// Get delivery by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Delivery</returns>
    /// <response code="200">Returns delivery with id</response>
    /// <response code="404">If the delivery not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DeliveryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Get all deliveries
    /// </summary>
    /// <param name="courierId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of deliveries</returns>
    /// <response code="200">Returns list of deliveries</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DeliveryResponse>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Cancel the delivery.
    /// </summary>
    /// <remarks>
    /// Cancel delivery and attached order.
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful cancel</response>
    /// <response code="400">If invalid delivery status</response>
    [HttpPatch("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelDelivery(
        Guid id,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new CancelDeliveryCommand(id),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Start process the delivery.
    /// </summary>
    /// <remarks>
    /// Courier take the delivery.
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="courierId"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful start process</response>
    /// <response code="400">If invalid delivery status</response>
    [HttpPatch("{id}/process")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartProcessDelivery(
        Guid id,
        Guid courierId,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new StartProcessDeliveryCommand(id, courierId),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Complete the delivery.
    /// </summary>
    /// <remarks>
    /// Complete the delivery and attached order.
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful complete</response>
    /// <response code="400">If invalid delivery status</response>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
