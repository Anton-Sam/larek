using BuildingBlocks.Domain;

namespace Catalog.Domain.BrandAggregate;

public sealed class Brand : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    private Brand(string name) : base(Guid.NewGuid())
    {
        Name = name;
    }

    public static Brand Create(string name)
    {
        return new Brand(name);
    }
}
