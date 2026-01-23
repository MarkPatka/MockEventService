using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        return services;
    }
    
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
