using Domain.Core.EventStore.Projections;
using Domain.Core.EventStore.Projections.Repositories;
using Shared.Optionals;

namespace Infrastructure.Repositories;

public class LastProcessedEventRepository : ILastProcessedEventRepository
{
    private readonly EventSourcingDbContext _eventSourcingDbContext;

    public LastProcessedEventRepository(EventSourcingDbContext eventSourcingDbContext)
    {
        _eventSourcingDbContext = eventSourcingDbContext;
    }

    public Optional<LastProcessedEvent> Get()
    {
        var result = _eventSourcingDbContext.LastProcessedEvents.SingleOrDefault();
        return result == null 
            ? Optional.Nothing<LastProcessedEvent>() 
            : Optional.Something(result);
    }

    public LastProcessedEvent Create()
    {
        var lastProcessedEvent = LastProcessedEvent.Create();
        _eventSourcingDbContext.LastProcessedEvents.Add(lastProcessedEvent);

        return lastProcessedEvent;
    }

    public void Update(LastProcessedEvent lastProcessedEvent)
    {
        _eventSourcingDbContext.LastProcessedEvents.Update(lastProcessedEvent);
    }
}