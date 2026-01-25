using MockEventService.Contracts.Events;

namespace MockEventService.Application.EventManagement.Common;

public record GetAllUserEventsResult(
    IEnumerable<EventDto> Events);
