using BuildingBlocks.Domain;

namespace Catalog.Domain.CategoryAggregate;

public sealed class Category : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    private Category(string name) : base(Guid.NewGuid())
    {
        Name = name;
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }
}
