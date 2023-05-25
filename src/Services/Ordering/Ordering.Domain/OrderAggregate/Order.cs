﻿using BuildingBlocks.Domain;

namespace Ordering.Domain.OrderAggregate;

public sealed class Order : AggregateRoot<Guid>
{
    public Guid BuyerId { get; private set; }
    public Address Address { get; private set; }
    public DeliveryType DeliveryType { get; private set; }
    public DateTime? DeliveryDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public uint TotalCount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public IEnumerable<OrderItem> Items => _items.AsReadOnly();


    private readonly List<OrderItem> _items = new();
    private Order(Guid buyerId, Address address, DeliveryType deliveryType, DateTime? deliveryDate = null)
        : base(Guid.NewGuid())
    {
        BuyerId = buyerId;
        Address = address;
        DeliveryType = deliveryType;
        Status = OrderStatus.Confirmed;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        DeliveryDate = deliveryDate;
    }

    public static Order CreatePickupOrder(
        Guid buyerId,
        Address address)
    {
        return new Order(buyerId, address, DeliveryType.Pickup);
    }

    public static Order CreateDeliveryOrder(
        Guid buyerId,
        Address address,
        DateTime? deliveryDate)
    {
        if (!deliveryDate.HasValue)
            throw new DomainException("Delivery date is required");

        return new Order(buyerId, address, DeliveryType.Delivery, deliveryDate);
    }

    public void AddOrderItem(
        Guid itemId,
        decimal price,
        uint count = 1)
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException(
                string.Format("Invalid order status: {0}", Status));

        TotalCount += count;
        TotalPrice += count * price;

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
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Confirmed)
        {
            throw new DomainException(
                string.Format("Invalid order status: {0}", Status));
        }

        Status = OrderStatus.Canceled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Fill()
    {
        if (Status != OrderStatus.Confirmed)
        {
            throw new DomainException(
                string.Format("Invalid order status: {0}", Status));
        }

        Status = OrderStatus.Filled;
        UpdatedAt = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private Order() { }
#pragma warning restore CS8618
}
