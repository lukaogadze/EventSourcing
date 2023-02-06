using Domain.Core;
using Domain.Core.EventStore;
using Domain.People.Models.Write;
using Shared;
using Shared.Optionals;

namespace Domain.People.Repository.Write;

public class PersonWriteRepository : IPersonWriteRepository
{
    private readonly IAppendOnlyStore _appendOnlyStore;

    public PersonWriteRepository(IAppendOnlyStore appendOnlyStore)
    {
        _appendOnlyStore = appendOnlyStore;
    }

    public Optional<Person> FindBy(Guid id)
    {
        ulong afterVersion = 0;
        const ulong eventCount = ulong.MaxValue;

        Optional<PersonSnapshot> latestSnapshot = _appendOnlyStore.GetLatestSnapshot<PersonSnapshot>(id);
        if (latestSnapshot.IsSomething)
        {
            afterVersion = latestSnapshot.Value.Version;
        }

        var storedEvents = _appendOnlyStore.GetStoredEvents(id, afterVersion, eventCount);
        if (storedEvents.IsNothing)
        {
            return Optional.Nothing<Person>();
        }

        if (latestSnapshot.IsSomething)
        {
            var snapshot = latestSnapshot.Value;
            var domainEvents = storedEvents.Value.Select(x => JsonService.Deserialize<DomainEvent>(x.Event).Value);
            var person = Person.Reconstruct(snapshot, domainEvents);

            return Optional.Something(person);
        }
        else
        {
            var domainEvents = storedEvents.Value.Select(x => JsonService.Deserialize<DomainEvent>(x.Event).Value);
            var person = Person.Reconstruct(domainEvents);

            return Optional.Something(person);
        }
    }

    public void Create(Person person)
    {
        _appendOnlyStore.CreateStream(person.Id, person.Changes);
    }

    public void CreateMany(IEnumerable<Person> entities)
    {
        foreach (var person in entities)
        {
            Create(person);
        }
    }
    
    public void Update(Person person)
    {
        var lastStoredEventExpectedVersion = person.StoredEventVersion == 0
            ? Optional.Nothing<ulong>()
            : Optional.Something(person.StoredEventVersion);

        _appendOnlyStore.AppendToStream(person.Id, person.Changes, lastStoredEventExpectedVersion);

        // TODO: test this motherfucker
        if (person.DomainEventVersion - person.StoredEventVersion >= IAppendOnlyStore.MinimumEventCountBetweenSnapshots)
        {
            SaveSnapshot(person);
        }
    }

    public void UpdateMany(IEnumerable<Person> entities)
    {
        foreach (var person in entities)
        {
            Update(person);
        }
    }

    public void SaveSnapshot(Person person)
    {
        _appendOnlyStore.AddSnapshot(person.Id, person.Snapshot());
    }
}