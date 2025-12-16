using Microsoft.Extensions.DependencyInjection;
using MockEventService.Application.Services;
using MockEventService.Infrastructure.Services;

namespace MockEventService.Infrastructure;

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
