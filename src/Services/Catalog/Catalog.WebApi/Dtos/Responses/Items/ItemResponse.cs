namespace Catalog.WebApi.Controllers.Dtos.Responses.Items;

public record ItemResponse(
    Guid Id,
    Guid BrandId,
    Guid CategoryId,
    decimal Price,
    string Name,
    string Description,
    uint AvailableCount,
    uint ReservedCount);
