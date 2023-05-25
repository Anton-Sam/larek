using Ordering.Application.Common.Interfaces;
using Ordering.Application.Models;
using Ordering.Domain.OrderAggregate;
using System.Text;
using System.Text.Json;

namespace Ordering.Infrastructure.Services;

public class DeliveryService : IDeliveryService
{
    private readonly HttpClient _httpClient;

    public DeliveryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Delivery?> CreateDeliveryAsync(Order order, CancellationToken cancellationToken)
    {
        var path = "/api/delivering/deliveries/";
        var body = new { orderId = order.Id, deliveryDate = order.DeliveryDate };
        var stringContent = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(path, new StringContent(
            stringContent,
            Encoding.UTF8,
            "application/json"));

        response.EnsureSuccessStatusCode();

        var content = await response.Content
            .ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<Delivery>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result;
    }
}
