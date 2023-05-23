using BuildingBlocks.Domain;

namespace Catalog.Domain.ItemAggregate;

public sealed class Item : AggregateRoot<Guid>
{
    public Guid BrandId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Price { get; private set; }
    public string Name { get; set; }
    public string Description { get; private set; }
    public uint AvailableCount { get; private set; }
    public uint ReservedCount { get; private set; }

    public Item(
        Guid brandId,
        Guid categoryId,
        decimal price,
        string name,
        string description,
        uint count) : base(Guid.NewGuid())
    {
        BrandId = brandId;
        CategoryId = categoryId;
        Price = price;
        Name = name;
        Description = description;
        AvailableCount = count;
    }

    public static Item Create(
        Guid brandId,
        Guid categoryId,
        decimal price,
        string name,
        string description,
        uint count)
    {
        return new Item(brandId,categoryId,price,name,description,count);
    }

    public void Reserve(uint count)
    {
        if (AvailableCount < count)
            throw new DomainException("Invalid items count");

        ReservedCount += count;
        AvailableCount -= count;
    }

    public void Sell(uint count)
    {
        if (ReservedCount < count)
            throw new DomainException("Invalid items count");

        ReservedCount -= count;
    }

    public void AddItems(uint count)
    {
        AvailableCount += count;
    }

    public void Release(uint count)
    {
        if (ReservedCount < count)
            throw new DomainException("Invalid items count");

        ReservedCount -= count;
        AvailableCount += count;
    }

#pragma warning disable CS8618
    private Item() { }
#pragma warning restore CS8618
}
