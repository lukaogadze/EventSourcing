using System.Collections.ObjectModel;
using Domain.Core;
using Domain.People.DomainEvents;

namespace Domain.People.Models.Write;

public class Person : EventSourcedAggregate
{
    public override ReadOnlyCollection<DomainEvent> Changes => _changes.AsReadOnly();
    private readonly List<DomainEvent> _changes = new();

    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public DateTimeOffset BirthDate { get; private set; }
    public string Hobby { get; private set; }
    
    public Person(
        Guid id,
        Name name,
        Address address,
        DateTimeOffset birthDate,
        string hobby)
    {
        Causes(new PersonCreated(
            id,
            name,
            address,
            birthDate,
            hobby
        ));
    }

    public void Update(
        Name name,
        Address address,
        DateTimeOffset birthDate,
        string hobby)
    {
        Causes(new PersonUpdated(
            name,
            address,
            birthDate,
            hobby
        ));
    }

    private Person(
        PersonSnapshot personSnapshot,
        IEnumerable<DomainEvent> storedDomainEvents)
    {
        DomainEventVersion = personSnapshot.Version;
        StoredEventVersion = personSnapshot.Version;
        
        StoredSnapshotVersion = personSnapshot.Version;

        Id = personSnapshot.AggregateId;
        Name = personSnapshot.Name;
        Address = personSnapshot.Address;
        BirthDate = personSnapshot.BirthDate;
        Hobby = personSnapshot.Hobby;


        foreach (DomainEvent @event in storedDomainEvents)
        {
            Apply(@event);
            StoredEventVersion++;
        }
    }

    private Person(IEnumerable<DomainEvent> storedDomainEvents)
    {
        foreach (DomainEvent @event in storedDomainEvents)
        {
            Apply(@event);
            StoredEventVersion++;
        }
    }
    
    public static Person Reconstruct(
        PersonSnapshot personSnapshot,
        IEnumerable<DomainEvent> storedDomainEvents)
    {
        return new Person(personSnapshot, storedDomainEvents);
    }

    public static Person Reconstruct(
        IEnumerable<DomainEvent> storedDomainEvents
    )
    {
        Person person = new Person(storedDomainEvents);

        return person;
    }

    public PersonSnapshot Snapshot() => new(
        DomainEventVersion,
        Id,
        Name,
        Address,
        BirthDate,
        Hobby
    );


    private void Causes(DomainEvent @event)
    {
        _changes.Add(@event);
        Apply(@event);
    }

    protected sealed override void Apply(DomainEvent @event)
    {
        When((dynamic)@event);
        DomainEventVersion++;
    }

    private void When(PersonCreated personCreated)
    {
        Id = personCreated.Id;
        Name = personCreated.Name;
        Address = personCreated.Address;
        BirthDate = personCreated.BirthDate;
        Hobby = personCreated.Hobby;
    }

    private void When(PersonUpdated personUpdated)
    {
        Name = personUpdated.Name;
        Address = personUpdated.Address;
        BirthDate = personUpdated.BirthDate;
        Hobby = personUpdated.Hobby;
    }
}