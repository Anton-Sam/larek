using Ordering.Application.Common.Interfaces;
using Ordering.Application.Models;
using System.Text.Json;

namespace Ordering.Infrastructure.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CatalogItem?> GetItemAsync(
        Guid itemId,
        CancellationToken cancellationToken = default)
    {
        var path = $"/api/catalog/items/{itemId}";
        var response = await _httpClient.GetAsync(path, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content
            .ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<CatalogItem>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result;
    }

    public async Task<bool> ReleaseItemsAsync(
        Guid itemId,
        uint count,
        CancellationToken cancellationToken = default)
    {
        var path = $"/api/catalog/items/{itemId}/release?count={count}";
        var response = await _httpClient.PatchAsync(
            path,
            null,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> ReserveItemsAsync(
        Guid itemId,
        uint count,
        CancellationToken cancellationToken = default)
    {
        var path = $"/api/catalog/items/{itemId}/reserve?count={count}";
        var response = await _httpClient.PatchAsync(
            path,
            null,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> SellItemsAsync(
        Guid itemId,
        uint count,
        CancellationToken cancellationToken = default)
    {
        var path = $"/api/catalog/items/{itemId}/sell?count={count}";
        var response = await _httpClient.PatchAsync(
            path,
            null,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return true;
    }
}
