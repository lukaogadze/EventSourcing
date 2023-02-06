namespace Domain.Core;

public abstract class DomainEvent
{
    public DateTimeOffset OccurredOn { get; }

    public DomainEvent()
    {
        OccurredOn = DateTimeOffset.Now;
    } 
}