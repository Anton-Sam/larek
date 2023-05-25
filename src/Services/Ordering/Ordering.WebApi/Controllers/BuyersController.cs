using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Buyers.Commands.CreateBuyer;
using Ordering.Application.Features.Buyers.Commands.GetBuyers;
using Ordering.Application.Features.Buyers.Queries.GetBuyerById;
using Ordering.WebApi.Dtos.Requests.Buyers;
using Ordering.WebApi.Dtos.Responses.Buyers;

namespace Ordering.WebApi.Controllers;

[Route("api/ordering/buyers")]
[Produces("application/json")]
public class BuyersController : ApiController
{
    public BuyersController(ISender sender) : base(sender)
    { }

    /// <summary>
    /// Create new buyer
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Newly created buyer</returns>
    /// <response code="201">Returns the newly created buyer</response>
    [HttpPost]
    [ProducesResponseType(typeof(BuyerResponse), 201)]
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

    /// <summary>
    /// Get all buyers
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of all buyers</returns>
    /// <response code="200">Returns list of all buyers</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BuyerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBuyers(CancellationToken cancellationToken)
    {
        var buyers = await Sender.Send(
            new GetBuyersQuery(),
            cancellationToken);

        var response = buyers.Select(b => new BuyerResponse(b.Id, b.Name));

        return Ok(response);
    }

    /// <summary>
    /// Get buyer by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Buyer</returns>
    /// <response code="200">Returns buyer with id</response>
    /// <response code="404">If the buyer not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BuyerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
