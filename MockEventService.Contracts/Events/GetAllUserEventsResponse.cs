using MockEventService.Contracts.DTO;

namespace MockEventService.Contracts.Events;

public sealed record GetAllUserEventsResponse(
    IEnumerable<EventDto> Events);


public sealed record EventDto(
    string Id,
    string Title,
    string Description,
    string EventType,
    string Status,
    LocationDto Location,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants);


