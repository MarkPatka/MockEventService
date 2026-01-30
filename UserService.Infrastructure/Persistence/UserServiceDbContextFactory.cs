using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.Infrastructure.Persistence;

public class UserServiceDbContextFactory
    : IDesignTimeDbContextFactory<UserServiceDbContext>
{
    public UserServiceDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<UserServiceDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=user_service;Username=postgres;Password=postgres")
            .Options;

        return new UserServiceDbContext(options);
    }
}