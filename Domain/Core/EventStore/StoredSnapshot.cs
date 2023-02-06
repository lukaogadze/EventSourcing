using Shared;

namespace Domain.Core.EventStore
{
    public class StoredSnapshot
    {
        public Guid Id { get; private set; }
        public Guid AggregateId { get; private set; }
        public string Snapshot { get; private set; }
        public DateTimeOffset CreateDate { get; private set; }

        private StoredSnapshot()
        {
        }
        
        public StoredSnapshot(Guid aggregateId, object snapshot)
        {
            Id = Guid.NewGuid();

            if (aggregateId == default)
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            AggregateId = aggregateId;

            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }
            Snapshot = JsonService.Serialize(snapshot);
            
            CreateDate = DateTimeOffset.Now;
        }
    }
}