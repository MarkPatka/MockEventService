using MockEventService.Domain.EventAggregate.Enumerations;

namespace MockEventService.Contracts.Events;

public sealed record CreateEventResponse(
    Guid EventId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    EventStatus Status);