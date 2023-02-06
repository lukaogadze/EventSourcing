namespace Domain.Core;

public abstract class DomainEvent
{
    public DateTimeOffset OccurredOn { get; }

    protected DomainEvent()
    {
        OccurredOn = DateTimeOffset.Now;
    } 
}