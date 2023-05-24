using Catalog.Application.Features.Brands.Commands.CreateBrand;
using Catalog.Application.Features.Brands.Queries.GetAllBrands;
using Catalog.Application.Features.Brands.Queries.GetBrandById;
using Catalog.Application.Features.Categories.Queries.GetCategoryById;
using Catalog.WebApi.Dtos.Requests.Brands;
using Catalog.WebApi.Dtos.Responses.Brands;
using Catalog.WebApi.Dtos.Responses.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("api/catalog/brands")]
public class BrandsController : ApiController
{
    public BrandsController(ISender sender) : base(sender)
    { }

    [HttpGet]
    public async Task<IActionResult> GetBrands(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllBrandsQuery(), cancellationToken);

        return Ok(result.Select(b => new BrandResponse(b.Id, b.Name)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(
        [FromBody] CreateBrandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateBrandCommand(request.Name),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetBrand),
            new { Id = result.Id },
            result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrand(
        Guid id,
        CancellationToken cancellationToken)
    {
        var brand = await Sender.Send(
            new GetBrandByIdQuery(id),
            cancellationToken);

        return Ok(new BrandResponse(brand.Id, brand.Name));
    }

}
