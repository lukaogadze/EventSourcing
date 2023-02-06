using Domain.Core.EventStore.Projections;
using Domain.Core.EventStore.Projections.Repositories;

namespace Infrastructure.Repositories;

public class ProcessedEventRepository : IProcessedEventRepository
{
    private readonly EventSourcingDbContext _eventSourcingDbContext;

    public ProcessedEventRepository(EventSourcingDbContext eventSourcingDbContext)
    {
        _eventSourcingDbContext = eventSourcingDbContext;
    }
    
    public void Create(ProcessedEvent processedEvent)
    {
        _eventSourcingDbContext.ProcessedEvents.Add(processedEvent);
    }
}