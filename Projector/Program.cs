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
        Console.WriteLine("--- Projector Is Running ---");
        while (true)
        {
            using var context = new EventSourcingDbContext();
            IProjectorService projectorService = new ProjectorService(
                new ReadRepositoryProvider(new PersonReadRepository(context)),
                new EventSourceRepository(context),
                new LastProcessedEventRepository(context),
                new ProcessedEventRepository(context)
            );

            projectorService.ProcessEvents();
            context.SaveChanges();
            context.Dispose();
            Thread.Sleep(2000);
        }
    }
}