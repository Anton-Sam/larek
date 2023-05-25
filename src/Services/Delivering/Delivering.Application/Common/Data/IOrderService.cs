namespace Delivering.Application.Common.Data;

public interface IOrderService
{
    Task<bool> CancelOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<bool> CompleteOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}
