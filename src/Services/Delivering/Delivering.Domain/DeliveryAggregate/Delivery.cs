using BuildingBlocks.Domain;

namespace Delivering.Domain.DeliveryAggregate;

public sealed class Delivery : AggregateRoot<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid? CourierId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime DeliveryDate { get; private set; }
    public DeliveryStatus Status { get; private set; }

    private Delivery(Guid orderId, DateTime deliveryDate)
        : base(Guid.NewGuid())
    {
        OrderId = orderId;
        DeliveryDate = deliveryDate;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = DeliveryStatus.ReadyToProcess;
    }

    public static Delivery Create(
        Guid orderId,
        DateTime deliveryDate)
    {
        return new Delivery(orderId, deliveryDate);
    }

    public void StartProcess(Guid courierId)
    {
        if (Status != DeliveryStatus.ReadyToProcess)
        {
            throw new DomainException(
                string.Format("Invalid delivery status: {0}", Status));
        }

        CourierId = courierId;
        UpdatedAt = DateTime.UtcNow;
        Status = DeliveryStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != DeliveryStatus.InProgress)
        {
            throw new DomainException(
                string.Format("Invalid delivery status: {0}", Status));
        }
        UpdatedAt = DateTime.UtcNow;
        Status = DeliveryStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == DeliveryStatus.Completed)
        {
            throw new DomainException(
                string.Format("Invalid delivery status: {0}", Status));
        }
        UpdatedAt = DateTime.UtcNow;
        Status = DeliveryStatus.Canceled;
    }
}
