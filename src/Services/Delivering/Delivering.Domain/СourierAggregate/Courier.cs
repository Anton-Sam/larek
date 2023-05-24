using BuildingBlocks.Domain;

namespace Delivering.Domain.СourierAggregate;

public sealed class Courier : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    private Courier(string name)
        : base(Guid.NewGuid())
    {
        Name = name;
    }

    public static Courier Create(string name)
    {
        return new Courier(name);
    }
}
