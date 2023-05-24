using BuildingBlocks.Domain;

namespace Ordering.Domain.OrderAggregate;

public sealed class OrderItem : Entity<Guid>
{
    public Guid ItemId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public uint UnitCount { get; private set; }

    private OrderItem(
        Guid itemId,
        decimal unitPrice,
        uint unitCount = 1)
    {
        ItemId = itemId;
        UnitPrice = unitPrice;
        UnitCount = unitCount;
    }

    public static OrderItem Create(
        Guid itemId,
        decimal unitPrice,
        uint unitCount = 1)
    {
        return new OrderItem(
            itemId,
            unitPrice,
            unitCount);
    }

    public void AddUnits(uint count)
    {
        UnitCount += count;
    }
}
