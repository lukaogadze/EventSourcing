using Domain.Core;

namespace Domain.People.Models.Write;

public class PersonSnapshot : Entity
{
    public ulong Version { get; private set; }
    public Guid AggregateId { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public DateTimeOffset BirthDate { get; private set; }
    public string Hobby { get; private set; }

    private PersonSnapshot()
    {
        
    }
    
    public PersonSnapshot(
        ulong version,
        Guid aggregateId, 
        Name name,
        Address address, 
        DateTimeOffset birthDate, 
        string hobby)
    {
        Id = Guid.NewGuid();
        Version = version;
        AggregateId = aggregateId;
        Name = name;
        Address = address;
        BirthDate = birthDate;
        Hobby = hobby;
    }
}