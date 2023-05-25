using Delivering.Application.Features.Couriers.Command.CreateCourier;
using Delivering.Application.Features.Couriers.Queries.GetCourierById;
using Delivering.Application.Features.Couriers.Queries.GetCouriers;
using Delivering.WebApi.Dtos.Requests.Couriers;
using Delivering.WebApi.Dtos.Responses.Couriers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivering.WebApi.Controllers;

[Route("/api/delivering/couriers")]
[Produces("application/json")]
public class CouriersController : ApiController
{
    public CouriersController(ISender sender) : base(sender)
    { }

    /// <summary>
    /// Create new courier
    /// </summary>
    /// <remarks>
    /// Courier can be attached to delivery
    /// </remarks>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Newly created courier</returns>
    /// <response code="201">Returns the newly created courier</response>
    [HttpPost]
    [ProducesResponseType(typeof(CourierResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCourier(
        [FromBody] CreateCourierRequest request,
        CancellationToken cancellationToken)
    {
        var courier = await Sender.Send(
            new CreateCourierCommand(request.Name),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetCourierById),
            new { Id = courier.Id },
            new CourierResponse(courier.Id, courier.Name));
    }

    /// <summary>
    /// Get all couriers
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of all couriers</returns>
    /// <response code="200">Returns list of all couriers</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourierResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCouriers(CancellationToken cancellationToken)
    {
        var courier = await Sender.Send(
            new GetCouriersQuery(),
            cancellationToken);

        var response = courier.Select(b => new CourierResponse(b.Id, b.Name));

        return Ok(response);
    }

    /// <summary>
    /// Get courier by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Courier</returns>
    /// <response code="200">Returns courier with id</response>
    /// <response code="404">If the courier not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourierById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var courier = await Sender.Send(
            new GetCourierByIdQuery(id),
            cancellationToken);

        return Ok(new CourierResponse(courier.Id, courier.Name));
    }
}
