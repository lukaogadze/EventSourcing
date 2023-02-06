namespace Domain.Core.EventStore.Projections;

public interface IProjectorService
{
    void ProcessEvents();
}