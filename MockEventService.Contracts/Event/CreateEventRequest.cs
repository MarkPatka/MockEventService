namespace MockEventService.Contracts.Event;

public sealed record CreateEventRequest(
    string Title,
    string Description);
