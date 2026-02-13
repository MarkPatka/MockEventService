using UserService.Application.IntegrationEvents.Clubs;
using UserService.Domain.ClubAggregate.DomainEvents;
using UserService.Domain.Common.Abstract;

namespace UserService.Application.IntegrationEvents.Mappers;

public static class IntegrationEventMapper
{
    public static IIntegrationEvent? Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            ClubCreatedDomainEvent e => new ClubCreatedIntegrationEvent
            {
                Id = e.ClubId.Value,
            },
            _ => null
        };
    }
}