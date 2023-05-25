﻿using Catalog.Application.Features.Items.Commands.AddItems;
using Catalog.Application.Features.Items.Commands.CreateItem;
using Catalog.Application.Features.Items.Commands.ReleaseItems;
using Catalog.Application.Features.Items.Commands.ReserveItems;
using Catalog.Application.Features.Items.Commands.SellItems;
using Catalog.Application.Features.Items.Queries.GetItemById;
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

    [HttpGet("{id}")]
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

    [HttpPatch("{id}/reserve/")]
    public async Task<IActionResult> ReserveItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new ReserveItemsCommand(id, count),
            cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/release")]
    public async Task<IActionResult> ReleaseItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new ReleaseItemsCommand(id, count),
            cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/sell")]
    public async Task<IActionResult> SellItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new SellItemsCommand(id, count),
            cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/add")]
    public async Task<IActionResult> AddItems(
        Guid id,
        uint count,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new AddItemsCommand(id, count),
            cancellationToken);

        return Ok();
    }
}
