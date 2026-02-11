using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Services;

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
        services.AddScoped<IClubService, ClubService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPublisher, IntegrationEventPublisher>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection RegisterDbContext(this IServiceCollection services)
    {
        services.AddDbContextFactory<UserServiceDbContext>(options =>
        {
            var connectionString = "Host=localhost;Port=5432;Database=user_service;Username=postgres;Password=postgres";
            options.UseNpgsql(connectionString, cfg => cfg.EnableRetryOnFailure(2));
        }, ServiceLifetime.Scoped);

        return services;
    }
}