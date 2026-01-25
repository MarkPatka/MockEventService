using Microsoft.EntityFrameworkCore;
using MockEventService.Domain.EventAggregate;

namespace MockEventService.Infrastructure.Persistence;

public class MockEventServiceDbContext(DbContextOptions<MockEventServiceDbContext> options) 
    : DbContext(options)
{
    public DbSet<Event> Events { get; set; } = null!;

}
