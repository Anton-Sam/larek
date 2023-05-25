using Catalog.Application.Features.Brands.Commands.CreateBrand;
using Catalog.Application.Features.Brands.Queries.GetAllBrands;
using Catalog.Application.Features.Brands.Queries.GetBrandById;
using Catalog.WebApi.Dtos.Requests.Brands;
using Catalog.WebApi.Dtos.Responses.Brands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("api/catalog/brands")]
[Produces("application/json")]
public class BrandsController : ApiController
{
    public BrandsController(ISender sender) : base(sender)
    { }


    /// <summary>
    /// Get all brands
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of all brands</returns>
    /// <response code="200">Returns list of all brands</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BrandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllBrandsQuery(), cancellationToken);

        return Ok(result.Select(b => new BrandResponse(b.Id, b.Name)));
    }

    /// <summary>
    /// Create new brand
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Newly created brand</returns>
    /// <response code="201">Returns the newly created brand</response>
    [HttpPost]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status201Created)]
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

    /// <summary>
    /// Get brand by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Brand</returns>
    /// <response code="200">Returns brand with id</response>
    /// <response code="404">If the brand not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
