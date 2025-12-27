using MockEventService.Domain;

namespace MockEventService.Contracts.Events;

public sealed record GetAllUserEventsResponse(IEnumerable<Event> Events);
