using Shared.Optionals;

namespace Domain.Core.EventStore
{
    public interface IAppendOnlyStore
    {
        const int EventCountBetweenSnapshots = 50;
        void AppendToStream(Guid aggregateId, IEnumerable<DomainEvent> domainEvents, Optional<ulong> lastStoredEventExpectedVersion);
        void CreateStream(Guid aggregateId, IEnumerable<DomainEvent> domainEvents);        
        Optional<IEnumerable<StoredEvent>> GetStoredEvents(Guid aggregateId, ulong afterVersion, ulong maxCount);
        void AddSnapshot<T>(Guid aggregateId, T snapshot);
        Optional<T> GetLatestSnapshot<T>(Guid aggregateId) where T : class;        
    }
}