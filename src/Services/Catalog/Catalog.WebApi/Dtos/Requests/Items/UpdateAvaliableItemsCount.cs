namespace Catalog.WebApi.Dtos.Requests.Items;

public record UpdateAvailableItemsCountRequest(
    Guid ItemId,
    uint Count);
