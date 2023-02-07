namespace Domain.Core;

public class DomainEvent
{
    public string Type { get; protected set; }
    public DateTimeOffset OccurredOn { get; protected set; }

    protected DomainEvent()
    {
        Type = GetType().Name;
        OccurredOn = DateTimeOffset.Now;
    } 
}