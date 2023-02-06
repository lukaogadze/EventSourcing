using Shared.Optionals;

namespace Domain.Core.EventStore.Projections.Repository;

public interface ILastProcessedEventRepository
{
    Optional<LastProcessedEvent> Get();
    void Update(LastProcessedEvent lastProcessedEvent);
}