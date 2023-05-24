using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Buyers.Commands.CreateBuyer;
using Ordering.Application.Features.Buyers.Commands.GetBuyers;
using Ordering.Application.Features.Buyers.Queries.GetBuyerById;
using Ordering.WebApi.Dtos.Requests.Buyers;
using Ordering.WebApi.Dtos.Responses.Buyers;

namespace Ordering.WebApi.Controllers;

[Route("api/ordering/buyers")]
public class BuyersController : ApiController
{
    public BuyersController(ISender sender) : base(sender)
    { }

    [HttpPost]
    public async Task<IActionResult> CreateBuyer(
        [FromBody] CreateBuyerRequest request,
        CancellationToken cancellationToken)
    {
        var buyer = await Sender.Send(
            new CreateBuyerCommand(request.Name),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetBuyerById),
            new { Id = buyer.Id },
            new BuyerResponse(buyer.Id, buyer.Name));
    }

    [HttpGet]
    public async Task<IActionResult> GetBuyers(CancellationToken cancellationToken)
    {
        var buyers = await Sender.Send(
            new GetBuyersQuery(),
            cancellationToken);

        var response = buyers.Select(b => new BuyerResponse(b.Id, b.Name));

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBuyerById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var buyer = await Sender.Send(
            new GetBuyerByIdQuery(id),
            cancellationToken);

        return Ok(new BuyerResponse(buyer.Id, buyer.Name));
    }
}
