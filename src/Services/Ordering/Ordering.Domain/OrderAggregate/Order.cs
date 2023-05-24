using BuildingBlocks.Domain;

namespace Ordering.Domain.OrderAggregate;

public sealed class Order : AggregateRoot<Guid>
{
    public Guid BuyerId { get; private set; }
    public Address Address { get; private set; }
    public DeliveryType DeliveryType { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public uint TotalCount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public IEnumerable<OrderItem> Items => _items.AsReadOnly();


    private readonly List<OrderItem> _items = new();
    private Order(Guid buyerId, Address address, DeliveryType deliveryType)
        : base(Guid.NewGuid())
    {
        BuyerId = buyerId;
        Address = address;
        DeliveryType = deliveryType;
        OrderStatus = OrderStatus.Confirmed;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Order Create(
        Guid buyerId,
        Address address,
        DeliveryType deliveryType)
    {
        return new Order(buyerId, address, deliveryType);
    }

    public void AddOrderItem(
        Guid itemId,
        decimal price,
        uint count = 1)
    {
        var existingItem = _items.SingleOrDefault(i => i.ItemId == itemId);

        if (existingItem is not null)
        {
            existingItem.AddUnits(count);
            return;
        }

        var item = OrderItem.Create(
                itemId,
                price,
                count);
        _items.Add(item);

        TotalCount += count;
        TotalPrice += count * price;
    }

    public void Cancel()
    {
        if (OrderStatus == OrderStatus.Confirmed)
        {
            OrderStatus = OrderStatus.Canceled;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Fill()
    {
        if (OrderStatus == OrderStatus.Confirmed)
        {
            OrderStatus = OrderStatus.Filled;
            UpdatedAt = DateTime.UtcNow;
        }
    }

#pragma warning disable CS8618
    private Order() { }
#pragma warning restore CS8618
}
