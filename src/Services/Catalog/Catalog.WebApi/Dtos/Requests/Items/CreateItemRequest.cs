﻿namespace Catalog.WebApi.Controllers.Dtos.Requests.Items;

public record CreateItemRequest(
    Guid BrandId,
    Guid CategoryId,
    decimal Price,
    string Name,
    string Description,
    uint Count);
