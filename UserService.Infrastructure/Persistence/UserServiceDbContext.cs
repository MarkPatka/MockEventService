using Microsoft.EntityFrameworkCore;
using UserService.Domain.ClubAggregate;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Infrastructure.Persistence;

public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
    : DbContext(options)
{
    public DbSet<Club> Clubs { get; set; } = null!;
    public DbSet<ClubMember> ClubMembers { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UserServiceDbContext).Assembly);
    }
}