namespace Domain.Core.EventStore.Projections;

public class LastProcessedEvent
{
    public Guid Id { get; private set; }

    public ulong StoredEventId { get; private set; }

    private LastProcessedEvent()
    {
    }
    
    public LastProcessedEvent(Guid id, ulong storedEventId)
    {
        Id = id;
        StoredEventId = storedEventId;
    }

    public void UpdateStoredEventId(ulong storedEventId)
    {
        StoredEventId = storedEventId;
    }
}