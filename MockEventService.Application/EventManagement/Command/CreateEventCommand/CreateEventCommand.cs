using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public sealed record CreateEventCommand(
    string Title,
    Guid EventTypeId,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    Guid OrganizerId,
    string? Description = null,
    Location? Location = null
    ) : IRequest<CreateEventResult>;
