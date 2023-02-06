namespace Domain.Core.EventStore.Projections;

public class ProcessedEvent
{
    public Guid Id { get; private set; }
    public Guid AggregateId { get; private set; }
    public Guid StoredEventId { get; private set; }
    public DateTimeOffset CreateDate { get; private set; }
    
    private ProcessedEvent() {}
    
    public ProcessedEvent(Guid id, Guid aggregateId, Guid storedEventId, DateTimeOffset createDate)
    {
        Id = id;
        AggregateId = aggregateId;
        StoredEventId = storedEventId;
        CreateDate = createDate;
    }
}