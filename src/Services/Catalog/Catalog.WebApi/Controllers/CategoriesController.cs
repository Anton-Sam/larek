using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Queries.GetAllCategories;
using Catalog.Application.Features.Categories.Queries.GetCategoryById;
using Catalog.WebApi.Dtos.Requests.Categories;
using Catalog.WebApi.Dtos.Responses.Categories;
using Catalog.WebApi.Dtos.Responses.Items;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Catalog.WebApi.Controllers;

[Route("api/catalog/categories")]
public class CategoriesController : ApiController
{
    public CategoriesController(ISender sender) : base(sender)
    { }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new GetAllCategoriesQuery(),
            cancellationToken);

        return Ok(result.Select(b => new CategoryResponse(b.Id, b.Name)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateCategoryCommand(request.Name),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetCategory),
            new { Id = result.Id },
            result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(
        Guid id,
        CancellationToken cancellationToken)
    {
        var category = await Sender.Send(
            new GetCategoryByIdQuery(id),
            cancellationToken);

        return Ok(new CategoryResponse(category.Id, category.Name));
    }
}
