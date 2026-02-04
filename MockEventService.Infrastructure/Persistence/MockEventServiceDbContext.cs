using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate;

namespace MockEventService.Infrastructure.Persistence;

public class MockEventServiceDbContext(DbContextOptions<MockEventServiceDbContext> options)
    : DbContext(options)
{
    public DbSet<Event> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MockEventServiceDbContext).Assembly);

        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.IsPrimaryKey())
            .ToList()
            .ForEach(e => e.ValueGenerated = ValueGenerated.Never);

        modelBuilder.Ignore<IDomainEvent>();
        
        base.OnModelCreating(modelBuilder);
    }

}
