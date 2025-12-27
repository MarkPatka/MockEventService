using MockEventService.Domain;

namespace MockEventService.Application.EventManagement.Common;

public record GetAllUserEventsResult(
    IEnumerable<Event> Events);
