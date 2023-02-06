using Domain.Core.EventStore;
using Domain.Core.EventStore.Projections;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class EventStoreDbContext : DbContext
{
    public DbSet<EventStream> EventStreams { get; set; }
    public DbSet<StoredEvent> StoredEvents { get; set; }
    public DbSet<StoredSnapshot> StoredSnapshots { get; set; }

    public DbSet<LastProcessedEvent> LastProcessedEvents { get; set; }

    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }
    
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventStream>().HasKey(x => x.AggregateId);
        modelBuilder.Entity<EventStream>().Property(x => x.AggregateId).ValueGeneratedNever();
        
        modelBuilder.Entity<StoredEvent>().HasKey(x => x.Id);
        modelBuilder.Entity<StoredEvent>().Property(x => x.Id).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<StoredSnapshot>().HasKey(x => x.Id);
        modelBuilder.Entity<StoredSnapshot>().Property(x => x.Id).ValueGeneratedNever();
        
        
        modelBuilder.Entity<ProcessedEvent>().HasKey(x => x.Id);
        modelBuilder.Entity<ProcessedEvent>().Property(x => x.Id).ValueGeneratedNever();
        
        modelBuilder.Entity<LastProcessedEvent>().HasKey(x => x.Id);
        modelBuilder.Entity<LastProcessedEvent>().Property(x => x.Id).ValueGeneratedNever();

        
        base.OnModelCreating(modelBuilder);
    }
}