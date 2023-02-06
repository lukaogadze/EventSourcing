namespace Domain.Core.EventStore.Projections.Repository;

public interface IEventSourceRepository
{
    List<StoredEvent> GetEventsFrom(ulong from);
}