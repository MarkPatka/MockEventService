using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Services;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices();
        return services;
    }


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITimeProviderService, TimeProviderService>();
        return services;
    }
}
