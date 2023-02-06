using Domain.Core;

namespace Domain.People.Models.Write;

public class Address : ValueObject
{
    public string City { get; private set; }
    public string Street { get; private set; }

    public Address(string city, string street)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentNullException(nameof(city));
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentNullException(nameof(street));
        }
        
        City = city;
        Street = street;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
    }
}