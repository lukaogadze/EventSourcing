using Shared.Optionals;

namespace Domain.Core.EventStore.Projections.Repositories;

public interface ILastProcessedEventRepository
{
    Optional<LastProcessedEvent> Get();
    LastProcessedEvent Create();
    void Update(LastProcessedEvent lastProcessedEvent);
}