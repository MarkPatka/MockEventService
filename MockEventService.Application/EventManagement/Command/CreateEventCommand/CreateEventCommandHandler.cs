using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.Entities;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public class CreateEventCommandHandler
    : IRequestHandler<CreateEventCommand, CreateEventResult>
{
    private readonly IEventService _eventService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(
        IEventService eventService, 
        IUnitOfWork unitOfWork)
    {
        _eventService = eventService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateEventResult> Handle(
        CreateEventCommand request,
        CancellationToken cancellationToken)
    {      
        var eventExists = await _eventService
            .CheckEventNotExists(request.Title, UserId.Create(request.OrganizerId), cancellationToken); /// NEW 

        if (eventExists)
            throw new Exception($"Event already exists");

        // var eventType = await _eventService.GetEventTypeAsync(request.EventTypeId);

        var newEvent = Event.Create(
            request.Title,
            request.Description!,
            eventType: EventType.CreateNew(
                    Guid.NewGuid(), 
                    "DotNext", 
                    "Conference", 
                    "Persisted_Icon_In_MiniO_Storage"),
            request.Location!,
            request.StartDate,
            request.EndDate,
            request.MaxParticipants,
            UserId.Create(request.OrganizerId),
            TimeProvider.System.GetUtcNow().DateTime,
            TimeProvider.System.GetUtcNow().DateTime
        );

        await _eventService.CreateEventAsync(newEvent, cancellationToken);

        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return new CreateEventResult(
            newEvent.Id.Value,
            newEvent.CreatedAt,
            newEvent.UpdatedAt,
            newEvent.Status.Id);
    }
}
