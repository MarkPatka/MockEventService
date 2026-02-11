using Microsoft.EntityFrameworkCore;
using UserService.Domain.ClubAggregate;
using UserService.Domain.UserProfileAggregate;
using UserService.Infrastructure.Persistence.Outbox;

namespace UserService.Infrastructure.Persistence;

public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
    : DbContext(options)
{
    public DbSet<Club> Clubs { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UserServiceDbContext).Assembly);
    }
}