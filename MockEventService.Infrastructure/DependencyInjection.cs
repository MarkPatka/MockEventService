using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MockEventService.Application.Common.Configuration;
using MockEventService.Application.EventSourcing;
using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.DomainEvents;
using MockEventService.Domain.EventAggregate.ValueObjects;
using MockEventService.Infrastructure.EventSourcing.EventHandlers;
using MockEventService.Infrastructure.EventSourcing.Messaging;
using MockEventService.Infrastructure.Persistence;
using MockEventService.Infrastructure.Services;

namespace MockEventService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddEventSourcing(configuration)
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

        services
            .AddScoped<IEventService, EventService>();

        return services;
    }


    private static IServiceCollection AddEventSourcing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventProducer, KafkaEventProducer>();
        services.AddSingleton<IEventConsumer, KafkaEventConsumer>();

        // Register domain event handlers 
        services.AddScoped<INotificationHandler<EventCreated>, EventCreatedDomainEventHandler>();
        services.AddScoped<INotificationHandler<EventPublished>, EventPublishedDomainEventHandler>();
        services.AddScoped<INotificationHandler<ParticipantRegistered>, ParticipantRegisteredDomainEventHandler>();

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
        services.AddDbContext<MockEventServiceDbContext>((provider, options) =>
        {
            var dbSettings = provider
                .GetRequiredService<IOptions<EventsDatabaseOptions>>().Value;

            options.UseNpgsql(dbSettings.CONNECTION_STRING, cfg => cfg.EnableRetryOnFailure(2));
        }, ServiceLifetime.Scoped);


        services.AddDbContextFactory<MockEventServiceDbContext>((provider, options) =>
        {
            var dbSettings = provider
                .GetRequiredService<IOptions<EventsDatabaseOptions>>().Value;

            options.UseNpgsql(dbSettings.CONNECTION_STRING, cfg => cfg.EnableRetryOnFailure(2));

        }, ServiceLifetime.Scoped);

        return services;
    }

}
