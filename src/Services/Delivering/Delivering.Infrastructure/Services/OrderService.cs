using Delivering.Application.Common.Data;

namespace Delivering.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CancelOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var path = $"/api/ordering/orders/{orderId}/cancel";
        var response = await _httpClient.PatchAsync(
            path,
            null,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> CompleteOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var path = $"/api/ordering/orders/{orderId}/complete";
        var response = await _httpClient.PatchAsync(
            path,
            null,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return true;
    }
}
