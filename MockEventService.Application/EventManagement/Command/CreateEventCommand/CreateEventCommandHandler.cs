using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.Persistence;
using MockEventService.Application.Persistence.Specifications;
using MockEventService.Application.Services;
using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public class CreateEventCommandHandler
    : IRequestHandler<CreateEventCommand, CreateEventResult>
{
    private readonly IRepository<Event, EventId> _repository;
    private readonly IEventService _eventService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeProviderService _timeProvider;

    public CreateEventCommandHandler(
        IRepository<Event, EventId> repository, 
        IEventService eventService, 
        ITimeProviderService timeProvider, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _eventService = eventService;
        _timeProvider = timeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateEventResult> Handle(
        CreateEventCommand request,
        CancellationToken cancellationToken)
    {
        /// OLD
        // check if not exists
        //var existingEventFromDb = await _repository.GetByIdAsync(x =>
        //        x.Title == request.Title &&
        //        x.EventType == request.EventType &&
        //        x.StartDate == request.StartDate &&
        //        x.OrganizerId == request.OrganizerId);
       
        var eventExists = await _eventService
            .CheckEventNotExists(request.Title, request.OrganizerId, cancellationToken); /// NEW 

        if (eventExists)
            throw new Exception($"Event already exists");

        // create
        var newEvent = Event.Create(
            request.Title,
            request.Description!,
            request.EventType,
            request.Location!,
            request.StartDate,
            request.EndDate,
            request.MaxParticipants,
            request.OrganizerId,
            _timeProvider.UtcNow,
            _timeProvider.UtcNow
        );

        // move to service!
        var newEntity = await _repository.AddAsync(newEvent, cancellationToken);
       
        await _unitOfWork.SaveEntitiesAsync(cancellationToken); /// NEW 

        // return result
        return new CreateEventResult(
            newEntity.Id.Value,
            newEntity.CreatedAt,
            newEntity.UpdatedAt,
            newEntity.Status.Id);
    }
}
