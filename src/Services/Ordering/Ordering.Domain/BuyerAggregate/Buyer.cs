using BuildingBlocks.Domain;

namespace Ordering.Domain.BuyerAggregate;

public class Buyer : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    private Buyer(string name)
        : base(Guid.NewGuid())
    {
        Name = name;
    }

    public static Buyer Create(string name)
    {
        return new Buyer(name);
    }
}
