using Delivering.Application.Features.Couriers.Command.CreateCourier;
using Delivering.Application.Features.Couriers.Queries.GetCourierById;
using Delivering.Application.Features.Couriers.Queries.GetCouriers;
using Delivering.WebApi.Dtos.Requests.Couriers;
using Delivering.WebApi.Dtos.Responses.Couriers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivering.WebApi.Controllers;

[Route("/api/delivering/couriers")]
public class CouriersController : ApiController
{
    public CouriersController(ISender sender) : base(sender)
    { }

    [HttpPost]
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

    [HttpGet]
    public async Task<IActionResult> GetCouriers(CancellationToken cancellationToken)
    {
        var courier = await Sender.Send(
            new GetCouriersQuery(),
            cancellationToken);

        var response = courier.Select(b => new CourierResponse(b.Id, b.Name));

        return Ok(response);
    }

    [HttpGet("{id}")]
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
