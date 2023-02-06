using System.Collections.ObjectModel;

namespace Domain.Core;

public abstract class EventSourcedAggregate : Entity
{
    protected abstract void Apply(DomainEvent @event);
    public abstract ReadOnlyCollection<DomainEvent> Changes { get; }
    public ulong DomainEventVersion { get; protected set; }
    public ulong StoredEventVersion { get; protected set; }
}