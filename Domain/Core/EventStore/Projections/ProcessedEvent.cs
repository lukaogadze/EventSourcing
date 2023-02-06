namespace Domain.Core.EventStore.Projections;

public class ProcessedEvent
{
    public Guid Id { get; private set; }
    public Guid AggregateId { get; private set; }
    public ulong StoredEventId { get; private set; }
    public DateTimeOffset CreateDate { get; private set; }
    
    private ProcessedEvent() {}
    
    public ProcessedEvent(Guid aggregateId, ulong storedEventId)
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        StoredEventId = storedEventId;
        CreateDate = DateTimeOffset.Now;
    }
}