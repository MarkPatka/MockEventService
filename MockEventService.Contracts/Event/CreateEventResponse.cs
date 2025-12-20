namespace MockEventService.Contracts.Event;

public sealed record CreateEventResponse(
    Guid EventId,
    DateTime CreatedAt);