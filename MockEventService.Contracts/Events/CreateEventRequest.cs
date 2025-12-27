namespace MockEventService.Contracts.Events;

public sealed record CreateEventRequest(
    string Title,
    string? Description);
