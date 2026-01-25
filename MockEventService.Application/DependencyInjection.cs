using Microsoft.Extensions.DependencyInjection;

namespace MockEventService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MUST REGISTER AND DOMAIN EVENTS AND CQRS - I HOPE...
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)); 


        return services;
    }
}
