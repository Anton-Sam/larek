using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Queries.GetAllCategories;
using Catalog.WebApi.Controllers.Dtos.Requests.Categories;
using Catalog.WebApi.Controllers.Dtos.Responses.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        //TODO 
        return Created("", result);
    }
}
