using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.Services;
using MockEventService.Contracts.DTO;
using MockEventService.Contracts.Events;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.EventManagement.Queries.GetAllUserEventsQuery;

public class GetAllUserEventsQueryHandler
    : IRequestHandler<GetAllUserEventsQuery, GetAllUserEventsResult>
{
    private readonly IEventService _eventService;

    public GetAllUserEventsQueryHandler(IEventService service)
    {
        _eventService = service;
    }

    public async Task<GetAllUserEventsResult> Handle(
        GetAllUserEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventService.GetEventsByOrganizerIdAsync(
            OrganizerId.Create(request.OrganizerId),
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var eventDtos = events.Select(e => new EventDto(
            e.Id.Value.ToString(),
            e.Title,
            e.Description,
            e.EventType.Name,
            e.Status.Name,
            new LocationDto(
                e.Location.Address, 
                e.Location.City, 
                e.Location.Country),
            e.StartDate,
            e.EndDate,
            e.MaxParticipants
        ));


        return new GetAllUserEventsResult(eventDtos);
    }
}
