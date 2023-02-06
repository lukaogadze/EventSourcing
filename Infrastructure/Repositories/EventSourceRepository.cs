using Domain.Core.EventStore;
using Domain.Core.EventStore.Projections.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventSourceRepository : IEventSourceRepository
{
    private readonly EventSourcingDbContext _eventSourcingDbContext;

    public EventSourceRepository(EventSourcingDbContext eventSourcingDbContext)
    {
        _eventSourcingDbContext = eventSourcingDbContext;
    }

    public List<StoredEvent> GetStoredEventsFrom(ulong from)
    {
        var events = _eventSourcingDbContext
            .StoredEvents
            .AsNoTracking()
            .Where(x => x.Id > from)
            .OrderBy(x => x.Id)
            .ToList();

        return events;
    }
}