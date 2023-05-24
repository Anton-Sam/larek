using Catalog.Application.Features.Items.Commands.CreateItem;
using Catalog.Application.Features.Items.Commands.UpdateAvailableItemCount;
using Catalog.Application.Features.Items.Queries.GetItems;
using Catalog.WebApi.Dtos.Requests.Items;
using Catalog.WebApi.Dtos.Responses.Items;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("/api/catalog/items")]
public class ItemsController : ApiController
{
    public ItemsController(ISender sender) : base(sender)
    { }

    [HttpGet]
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

    [HttpPost]
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

        //TODO
        return Created("", new ItemResponse(
            item.Id,
            item.BrandId,
            item.CategoryId,
            item.Price,
            item.Name,
            item.Description,
            item.AvailableCount,
            item.ReservedCount));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAvailableCount(
        [FromBody] UpdateAvailableItemsCountRequest request,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new UpdateAvailableItemCountCommand(
                request.ItemId,
                request.Count), cancellationToken);

        return Ok();
    }
}
