using Shared;

namespace Domain.Core.EventStore
{
    public class StoredEvent
    {
        public Guid Id { get; private set; }
        public DateTimeOffset CreateDate { get; private set; }
        public string Event { get; private set; }
        public ulong Version { get; private set; }
        public Guid AggregateId { get; private set; }

        private StoredEvent()
        {
        }

        public StoredEvent(Guid aggregateId, DomainEvent @event, ulong lastStoredEventVersion)
        {
            Id = Guid.NewGuid();

            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (@event.OccurredOn == default)
            {
                throw new ArgumentNullException(nameof(@event.OccurredOn));
            }

            CreateDate = @event.OccurredOn;
            Event = JsonService.Serialize(@event);


            if (aggregateId == default)
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }

            AggregateId = aggregateId;

            if (lastStoredEventVersion <= 0)
            {
                throw new InvalidOperationException($"{nameof(lastStoredEventVersion)} should not be negative or zero");
            }

            Version = lastStoredEventVersion;
        }
    }
}