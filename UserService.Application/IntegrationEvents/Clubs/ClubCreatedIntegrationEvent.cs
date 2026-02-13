using System;

namespace UserService.Application.IntegrationEvents.Clubs;

public class ClubCreatedIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}