namespace BuildingBlocks.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; protected set; }

    protected Entity(TId id) => Id = id;

    public override bool Equals(object? obj) =>
        obj is Entity<TId> entity && Id.Equals(entity.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public bool Equals(Entity<TId>? other) =>
        other is not null && Id.Equals(other.Id);

#pragma warning disable CS8618
    protected Entity() { }
#pragma warning restore CS8618
}