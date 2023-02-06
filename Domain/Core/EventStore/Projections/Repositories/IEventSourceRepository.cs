namespace Domain.Core.EventStore.Projections.Repositories;

public interface IEventSourceRepository
{
    List<StoredEvent> GetStoredEventsFrom(ulong from);
}