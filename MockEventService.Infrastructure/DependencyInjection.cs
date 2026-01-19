using Microsoft.Extensions.DependencyInjection;
using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Infrastructure.Persistence;
using MockEventService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace MockEventService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddServices()
            .RegisterRepositories()
            ;
        return services;
    }


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ITimeProviderService, TimeProviderService>();
        
        services
            .AddTransient<IMockConfigurationService, MockConfigurationService>();


        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {

        services
            .AddScoped<IEventRepository, EventRepository>();

        return services;
    }

}
