namespace Domain.Core.EventStore
{
    public class EventStream
    {
        public Guid AggregateId { get; private set; }
        public ulong LastStoredEventVersion { get; private set; }
        public DateTimeOffset CreateDate { get; set; }

        private EventStream()
        {
            
        }
        
        public EventStream(Guid aggregateId)
        {
            if (default == aggregateId)
            {
                throw new InvalidOperationException($"{nameof(aggregateId)} must be initialized");
            }            
            AggregateId = aggregateId;            
            CreateDate = DateTimeOffset.Now;
        }

        public StoredEvent RegisterStoredEvent(DomainEvent domainEvent)
        {
            LastStoredEventVersion++;
            return new StoredEvent(AggregateId, domainEvent, LastStoredEventVersion);
        }
    }
}