namespace Domain.Core.EventStore.Projections.Repositories;

public interface IProcessedEventRepository
{
    void Create(ProcessedEvent processedEvent);
}