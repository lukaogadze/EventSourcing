using Domain.Core;
using Domain.People.Models.Write;

namespace Domain.People.DomainEvents;

public class PersonCreated : DomainEvent
{
    public Guid Id { get; }
    public Name Name { get; }
    public Address Address { get; }
    public DateTimeOffset BirthDate { get; }
    public string Hobby { get; }

    public PersonCreated(Guid id, Name name, Address address, DateTimeOffset birthDate, string hobby)
    {
        Id = id;
        Name = name;
        Address = address;
        BirthDate = birthDate;
        Hobby = hobby;
    }
}