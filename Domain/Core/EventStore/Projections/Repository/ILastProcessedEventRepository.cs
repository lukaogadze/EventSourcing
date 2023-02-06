using Domain.Core.Repository;

namespace Domain.Core.EventStore.Projections.Repository;

public interface ILastProcessedEventRepository : IWriteRepository<LastProcessedEvent>
{
}