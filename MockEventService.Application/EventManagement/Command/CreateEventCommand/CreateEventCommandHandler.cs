using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Domain;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public class CreateEventCommandHandler
    : IRequestHandler<CreateEventCommand, CreateEventResult>
{
    private readonly IEventRepository _repository;
    private readonly ITimeProviderService _timeProvider;


    public CreateEventCommandHandler(IEventRepository repository, ITimeProviderService timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }


    public Task<CreateEventResult> Handle(
        CreateEventCommand request, 
        CancellationToken cancellationToken)
    {
        // check if not exists

        // create

        // add

        // return result

        return Task.FromResult(new CreateEventResult(Guid.NewGuid(), _timeProvider.UtcNow));
    }
}
