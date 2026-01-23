using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<IClubMembersRepository, ClubMembersRepository>();
        return services;
    }


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
