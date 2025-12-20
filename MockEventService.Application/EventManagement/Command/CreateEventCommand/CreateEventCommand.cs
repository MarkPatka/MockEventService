using MediatR;
using MockEventService.Application.EventManagement.Common;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public record CreateEventCommand(
    string Title, string? Description = null) 
    : IRequest<CreateEventResult>;
