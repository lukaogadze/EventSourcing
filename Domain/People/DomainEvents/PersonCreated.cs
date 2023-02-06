using Domain.Core;
using Domain.People.Models.Write;

namespace Domain.People.DomainEvents;

public class PersonCreated : DomainEvent
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public DateTimeOffset BirthDate { get; private set; }
    public string Hobby { get; private set; }

    public PersonCreated(Guid id, Name name, Address address, DateTimeOffset birthDate, string hobby)
    {
        Id = id;
        Name = name;
        Address = address;
        BirthDate = birthDate;
        Hobby = hobby;
    }
}