using Domain;
using Domain.Core;
using Domain.Core.EventStore;
using Domain.Core.EventStore.Projections;
using Domain.Core.EventStore.Projections.Repositories;
using Domain.People.DomainEvents;
using Domain.People.Models.Read;
using Shared;

namespace Infrastructure.Services;

public class ProjectorService : IProjectorService
{
    private readonly ReadRepositoryProvider _readRepositoryProvider;
    private readonly IEventSourceRepository _eventSourceRepository;
    private readonly ILastProcessedEventRepository _lastProcessedEventRepository;
    private readonly IProcessedEventRepository _processedEventRepository;

    public ProjectorService(
        ReadRepositoryProvider readRepositoryProvider,
        IEventSourceRepository eventSourceRepository,
        ILastProcessedEventRepository lastProcessedEventRepository,
        IProcessedEventRepository processedEventRepository)
    {
        _readRepositoryProvider = readRepositoryProvider;
        _eventSourceRepository = eventSourceRepository;
        _lastProcessedEventRepository = lastProcessedEventRepository;
        _processedEventRepository = processedEventRepository;
    }

    public void ProcessEvents()
    {
        var lastProcessedEvent = GetLastProcessedEvent();

        var events = _eventSourceRepository.GetStoredEventsFrom(lastProcessedEvent.StoredEventId);

        foreach (var @event in events)
        {
            ProcessEventsInternal(@event);
        }

        if (events.Any())
        {
            var lastStoredEventId = events.Last().Id;
            lastProcessedEvent.UpdateStoredEventId(lastStoredEventId);
            _lastProcessedEventRepository.Update(lastProcessedEvent);
        }
    }

    private void ProcessEventsInternal(StoredEvent @event)
    {
        var aggregateId = @event.AggregateId;
        switch (@event.Type)
        {
            case nameof(PersonCreated):
                var personCreated = JsonService.Deserialize<PersonCreated>(@event.DomainEvent).Value;
                CreatePerson(personCreated, aggregateId);
                break;
            
            case nameof(PersonUpdated):
                var personUpdated = JsonService.Deserialize<PersonUpdated>(@event.DomainEvent).Value;
                UpdatePerson(personUpdated, aggregateId);
                break;
        }

        var processedEvent = new ProcessedEvent(
            aggregateId,
            @event.Id
        );
        _processedEventRepository.Create(processedEvent);
    }

    private void UpdatePerson(PersonUpdated personUpdated, Guid aggregateId)
    {
        var person = _readRepositoryProvider
            .People
            .GetFirstOrDefault(x => x.Id == aggregateId)
            .Value;
        
        person.Update(
            personUpdated.Name.FirstName,
            personUpdated.Name.MiddleName,
            personUpdated.Name.LastName,
            personUpdated.Address.City,
            personUpdated.Address.Street,
            personUpdated.BirthDate,
            personUpdated.Hobby
        );
        
        _readRepositoryProvider.People.Update(person);
    }

    private void CreatePerson(PersonCreated personCreated, Guid aggregateId)
    {
        _readRepositoryProvider.People.Create(new PersonReadModel(
            personCreated.Id,
            personCreated.Name.FirstName,
            personCreated.Name.MiddleName,
            personCreated.Name.LastName,
            personCreated.Address.City,
            personCreated.Address.Street,
            personCreated.BirthDate,
            personCreated.Hobby
        ));
    }

    private LastProcessedEvent GetLastProcessedEvent()
    {
        LastProcessedEvent lastProcessedEvent;
        var lastProcessedEventOptional = _lastProcessedEventRepository.Get();
        
        if (lastProcessedEventOptional.IsNothing)
        {
            lastProcessedEvent = _lastProcessedEventRepository.Create();
        }
        else
        {
            lastProcessedEvent = lastProcessedEventOptional.Value;
        }

        return lastProcessedEvent;
    }
}