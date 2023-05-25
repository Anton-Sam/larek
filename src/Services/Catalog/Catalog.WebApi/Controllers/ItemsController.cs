using Catalog.Application.Features.Items.Commands.AddItems;
using Catalog.Application.Features.Items.Commands.CreateItem;
using Catalog.Application.Features.Items.Commands.ReleaseItems;
using Catalog.Application.Features.Items.Commands.ReserveItems;
using Catalog.Application.Features.Items.Commands.SellItems;
using Catalog.Application.Features.Items.Queries.GetItemById;
using Catalog.Application.Features.Items.Queries.GetItems;
using Catalog.WebApi.Dtos.Requests.Items;
using Catalog.WebApi.Dtos.Responses.Brands;
using Catalog.WebApi.Dtos.Responses.Categories;
using Catalog.WebApi.Dtos.Responses.Items;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("/api/catalog/items")]
[Produces("application/json")]
public class ItemsController : ApiController
{
    public ItemsController(ISender sender) : base(sender)
    { }

    /// <summary>
    /// Get all items
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of all items</returns>
    /// <response code="200">Returns list of all items</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
    {
        var items = await Sender.Send(
            new GetItemsQuery(),
            cancellationToken);

        var result = items.Select(i => new ItemResponse(
            i.Id,
            i.BrandId,
            i.CategoryId,
            i.Price,
            i.Name,
            i.Description,
            i.AvailableCount,
            i.ReservedCount));

        return Ok(result);
    }

    /// <summary>
    /// Create new item
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>New created item</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /items
    ///     {
    ///         "brandId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "price": 10,
    ///         "name": "Air Force",
    ///         "description": "Best sneakers",
    ///         "count": 10
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="404">If brand or category not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateItem(
        [FromBody] CreateItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await Sender.Send(
            new CreateItemCommand(
                request.BrandId,
                request.CategoryId,
                request.Price,
                request.Name,
                request.Description,
                request.Count));

        return CreatedAtAction(nameof(GetItem), new { Id = item.Id }, new ItemResponse(
            item.Id,
            item.BrandId,
            item.CategoryId,
            item.Price,
            item.Name,
            item.Description,
            item.AvailableCount,
            item.ReservedCount));
    }


    /// <summary>
    /// Get item by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Item</returns>
    /// <response code="200">Returns item with id</response>
    /// <response code="404">If the item not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItem(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await Sender.Send(
            new GetItemByIdQuery(id),
            cancellationToken);

        var response = new ItemResponse(
            item.Id,
            item.BrandId,
            item.CategoryId,
            item.Price,
            item.Name,
            item.Description,
            item.AvailableCount,
            item.ReservedCount);

        return Ok(response);
    }

    /// <summary>
    /// Reserve number of items.
    /// </summary>
    /// <remarks>
    /// Using by the Order service when new order is created
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful update</response>
    /// <response code="400">If invalid count</response>
    [HttpPatch("{id}/reserve/")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReserveItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new ReserveItemsCommand(id, count),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Release number of items.
    /// </summary>
    /// <remarks>
    /// Using by the Order service when order is canceled
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful update</response>
    /// <response code="400">If invalid count</response>
    [HttpPatch("{id}/release")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReleaseItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new ReleaseItemsCommand(id, count),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Sell number of items.
    /// </summary>
    /// <remarks>
    /// Using by the Order service when order is filled
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful update</response>
    /// <response code="400">If invalid count</response>
    [HttpPatch("{id}/sell")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SellItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new SellItemsCommand(id, count),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Add number of items.
    /// </summary>
    /// <remarks>
    /// Used by the seller to replenish current positions
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful update</response>
    /// <response code="400">If invalid count</response>
    [HttpPatch("{id}/add")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new AddItemsCommand(id, count),
            cancellationToken);

        return NoContent();
    }
}
