using Domain.Core;
using Domain.People.Models.Write;

namespace Domain.People.DomainEvents;

public class PersonUpdated : DomainEvent
{
    public Name Name { get; private set;}
    public Address Address { get; private set;}
    public DateTimeOffset BirthDate { get; private set;}
    public string Hobby { get; private set; }

    public PersonUpdated(Name name, Address address, DateTimeOffset birthDate, string hobby)
    {
        Name = name;
        Address = address;
        BirthDate = birthDate;
        Hobby = hobby;
    }
}