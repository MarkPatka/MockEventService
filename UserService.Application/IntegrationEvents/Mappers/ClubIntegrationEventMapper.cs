using UserService.Application.IntegrationEvents.Clubs;
using UserService.Domain.ClubAggregate;

namespace UserService.Application.IntegrationEvents.Mappers;

public static class ClubIntegrationEventMapper
{
    public static ClubCreatedIntegrationEvent ToIntegrationEvent(Club club)
    {
        return new ClubCreatedIntegrationEvent
        {
            ClubId = club.Id.Value,
            Name = club.Name,
            OwnerId = club.Owner.Value
        };
    }
}