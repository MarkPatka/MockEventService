using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MockEventService.Application.Common.Configuration;
using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.ValueObjects;
using MockEventService.Infrastructure.Persistence;
using MockEventService.Infrastructure.Services;

namespace MockEventService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddServices()
            .RegisterDbContext()
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
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IRepository<Event, EventId>, GenericRepository<Event, EventId>>()
            ;


        return services;
    }

    private static IServiceCollection RegisterDbContext(this IServiceCollection services)
    {
        // USUALLY WE DO SMTH LIKE THIS: .AddDbContext<MockEventServiceDbContext>()

        services.AddDbContextFactory<MockEventServiceDbContext>((provider, options) =>
        {
            var dbSettings = provider
                .GetRequiredService<IOptions<EventsDatabaseConnection>>().Value;

            options.UseNpgsql(dbSettings.CONNECTION_STRING, cfg => cfg.EnableRetryOnFailure(2));

        }, ServiceLifetime.Scoped);

        return services;
    }

}
