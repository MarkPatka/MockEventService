using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Domain.EventAggregate;

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
            .CheckEventNotExists(request.Title, request.OrganizerId, cancellationToken); /// NEW 

        if (eventExists)
            throw new Exception($"Event already exists");

        var newEvent = Event.Create(
            request.Title,
            request.Description!,
            request.EventType,
            request.Location!,
            request.StartDate,
            request.EndDate,
            request.MaxParticipants,
            request.OrganizerId,
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
