using MockEventService.Application.EventManagement.Command.CreateEventCommand;
using MockEventService.Application.Persistence;
using MockEventService.Application.Persistence.Specifications;
using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.Services;

public interface IEventService
{
    public Task<IReadOnlyList<Event>> GetEventsByOrganizerIdAsync(OrganizerId id, int pageNumber, int pageSize, CancellationToken cancellationToken);
    public Task<bool> CheckEventNotExists(string title, OrganizerId request, CancellationToken cancellationToken);
    public Task<Event?> GetEventByIdAsync(EventId id, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<Event>> GetActiveEventsAsync(CancellationToken cancellationToken = default);
    public Task<Event> CreateEventAsync(Event @event, CancellationToken cancellationToken);
}
