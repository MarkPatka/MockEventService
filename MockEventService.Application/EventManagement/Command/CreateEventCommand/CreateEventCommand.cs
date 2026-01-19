using MediatR;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Domain.EventAggregate.Entities;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public record CreateEventCommand(
    string Title,
    EventType EventType,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    OrganizerId OrganizerId,
    string? Description = null,
    Location? Location = null
    ) : IRequest<CreateEventResult>;
