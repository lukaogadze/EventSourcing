using Domain.Core;
using Domain.People.Models.Write;

namespace Domain.People.DomainEvents;

public class PersonUpdated : DomainEvent
{
    public Name Name { get; }
    public Address Address { get; }
    public DateTimeOffset BirthDate { get; }
    public string Hobby { get; }

    public PersonUpdated(Name name, Address address, DateTimeOffset birthDate, string hobby)
    {
        Name = name;
        Address = address;
        BirthDate = birthDate;
        Hobby = hobby;
    }
}