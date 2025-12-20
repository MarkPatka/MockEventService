namespace MockEventService.Application.EventManagement.Common;

public record CreateEventResult(
    Guid EventId,
    DateTime CreatedAt);
