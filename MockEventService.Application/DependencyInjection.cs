using Microsoft.Extensions.DependencyInjection;

namespace MockEventService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        //services.Add(...)
        // ...
        return services;
    }
}
