using BuildingBlocks.Domain;

namespace Ordering.Domain.OrderAggregate;

public sealed class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    public Address(
        string street,
        string city,
        string state,
        string country,
        string zipcode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipcode;
    }

    public static Address Create(
        string street,
        string city,
        string state,
        string country,
        string zipcode)
    {
        return new Address(street, city, state, country, zipcode);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }

#pragma warning disable CS8618
    private Address() { }
#pragma warning restore CS8618

}
