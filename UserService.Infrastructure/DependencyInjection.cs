using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices();
        services.RegisterRepositories();
        services.RegisterDbContext();
        return services;
    }


    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Club, ClubId>, GenericRepository<Club, ClubId>>();
        services.AddScoped<IRepository<UserProfile, UserId>, GenericRepository<UserProfile, UserId>>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection RegisterDbContext(this IServiceCollection services)
    {
        services.AddDbContextFactory<UserServiceDbContext>((provider, options) =>
        {
            //TODO
            var connectionString = string.Empty;

            options.UseNpgsql(connectionString, cfg => cfg.EnableRetryOnFailure(2));
        }, ServiceLifetime.Scoped);

        return services;
    }
}