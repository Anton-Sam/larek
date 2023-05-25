using Ordering.Application.Models;

namespace Ordering.Application.Common.Interfaces;

public interface ICatalogService
{
    Task<CatalogItem?> GetItemAsync(
        Guid itemId,
        CancellationToken cancellationToken = default);

    Task<bool> ReserveItemsAsync(
        Guid itemId,
        uint count,
        CancellationToken cancellationToken = default);

    Task<bool> ReleaseItemsAsync(
        Guid itemId,
        uint count,
        CancellationToken cancellationToken = default);

    Task<bool> SellItemsAsync(
        Guid itemId,
        uint count,
        CancellationToken cancellationToken = default);
}
