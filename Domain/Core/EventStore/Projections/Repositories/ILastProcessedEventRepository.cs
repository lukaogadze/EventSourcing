using Shared.Optionals;

namespace Domain.Core.EventStore.Projections.Repositories;

public interface ILastProcessedEventRepository
{
    Optional<LastProcessedEvent> Get();
    void Create();
    void Update(LastProcessedEvent lastProcessedEvent);
}