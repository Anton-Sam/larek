using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Queries.GetAllCategories;
using Catalog.Application.Features.Categories.Queries.GetCategoryById;
using Catalog.WebApi.Dtos.Requests.Categories;
using Catalog.WebApi.Dtos.Responses.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("api/catalog/categories")]
[Produces("application/json")]
public class CategoriesController : ApiController
{
    public CategoriesController(ISender sender) : base(sender)
    { }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of all categories</returns>
    /// <response code="200">Returns list of all categories</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new GetAllCategoriesQuery(),
            cancellationToken);

        return Ok(result.Select(b => new CategoryResponse(b.Id, b.Name)));
    }

    /// <summary>
    /// Create new category
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>New created category</returns>
    /// <response code="201">Returns the newly created category</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
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

    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Category</returns>
    /// <response code="200">Returns category with id</response>
    /// <response code="404">If the category not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryResponse), 201)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
