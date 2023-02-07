using Domain;
using Domain.Core.EventStore.Projections;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Projector;

public static class Program
{
    public static void Main()
    {
        using var context = new EventSourcingDbContext();

        IProjectorService projectorService = new ProjectorService(
            new ReadRepositoryProvider(new PersonReadRepository(context)),
            new EventSourceRepository(context),
            new LastProcessedEventRepository(context),
            new ProcessedEventRepository(context)
        );

        while (true)
        {
            projectorService.ProcessEvents();
            context.SaveChanges();
            Thread.Sleep(2000);
        }
    }
}