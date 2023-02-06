using Domain.Core;
using Domain.Core.EventStore;
using Shared;
using Shared.Optionals;

namespace Infrastructure.EventStore;

public class EventStore : IAppendOnlyStore
{
    private readonly EventStoreDbContext _eventStoreDbContext;

    public EventStore(EventStoreDbContext eventStoreDbContext)
    {
        _eventStoreDbContext = eventStoreDbContext;
    }

    public void AppendToStream(
        Guid aggregateId,
        IEnumerable<DomainEvent> domainEvents,
        Optional<ulong> lastStoredEventExpectedVersion)
    {
        var eventStream = _eventStoreDbContext.EventStreams
            .SingleOrDefault(x => x.AggregateId == aggregateId);

        if (eventStream == null)
        {
            throw new InvalidOperationException("event stream not found with aggregateId " + aggregateId);
        }

        if (lastStoredEventExpectedVersion.IsSomething)
        {
            CheckForConcurrency(lastStoredEventExpectedVersion.Value, eventStream.LastStoredEventVersion);
        }

        var storedEvents = new List<StoredEvent>();
        foreach (DomainEvent @event in domainEvents)
        {
            var storedEvent = eventStream.RegisterStoredEvent(@event);
            storedEvents.Add(storedEvent);
        }

        _eventStoreDbContext.StoredEvents.AddRange(storedEvents);
        _eventStoreDbContext.EventStreams.Update(eventStream);
    }

    private void CheckForConcurrency(ulong expectedVersion, ulong lastStoredEventVersion)
    {
        if (lastStoredEventVersion == expectedVersion)
        {
            return;
        }

        var error = $"Expected: {expectedVersion}. Found: {lastStoredEventVersion}";
        throw new OptimisticConcurrencyException(error);
    }

    public void CreateStream(Guid aggregateId, IEnumerable<DomainEvent> domainEvents)
    {
        var eventStream = new EventStream(aggregateId);
        _eventStoreDbContext.EventStreams.Add(eventStream);

        AppendToStream(eventStream.AggregateId, domainEvents, Optional.Nothing<ulong>());
    }

    public Optional<IEnumerable<StoredEvent>> GetStoredEvents(Guid aggregateId, ulong afterVersion, ulong maxCount)
    {
        var eventStream = _eventStoreDbContext.EventStreams.SingleOrDefault(x => x.AggregateId == aggregateId);
        if (eventStream == null)
        {
            return Optional.Nothing<IEnumerable<StoredEvent>>();
        }

        var storedEvents = _eventStoreDbContext.StoredEvents
            .Where(x => x.AggregateId == aggregateId && x.Version > afterVersion)
            .Take((int)maxCount)
            .AsEnumerable();

        return Optional.Something(storedEvents);
    }

    public void AddSnapshot<T>(Guid aggregateId, T snapshot)
    {
        var eventStream = _eventStoreDbContext.EventStreams
            .SingleOrDefault(x => x.AggregateId == aggregateId);

        if (eventStream == null)
        {
            throw new InvalidOperationException("event stream not found with aggregateId " + aggregateId);
        }

        var storedSnapshot = new StoredSnapshot(eventStream.AggregateId, snapshot);

        _eventStoreDbContext.StoredSnapshots.Add(storedSnapshot);
    }

    public Optional<T> GetLatestSnapshot<T>(Guid aggregateId) where T : class
    {
        var storedSnapshot = _eventStoreDbContext.StoredSnapshots
            .Where(x => x.AggregateId == aggregateId)
            .OrderByDescending(x => x.CreateDate)
            .FirstOrDefault();

        if (storedSnapshot == null)
        {
            return Optional.Nothing<T>();
        }

        return Optional.Something(JsonService.Deserialize<T>(storedSnapshot.Snapshot).Value);
    }
}