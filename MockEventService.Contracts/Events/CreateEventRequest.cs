using MockEventService.Domain.EventAggregate.Entities;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Contracts.Events;

public sealed record CreateEventRequest(
    string Title,
    EventType EventType,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    OrganizerId OrganizerId,
    string? Description,
    Location? Location);
