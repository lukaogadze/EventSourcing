namespace Domain.Core.EventStore.Projections;

public class LastProcessedEvent
{
    public Guid Id { get; private set; }

    public ulong StoredEventId { get; private set; }

    private LastProcessedEvent()
    {
    }

    public static LastProcessedEvent Create() => new()
    {
        Id = Guid.NewGuid(),
        StoredEventId = 0
    };

    public void UpdateStoredEventId(ulong storedEventId)
    {
        StoredEventId = storedEventId;
    }
}