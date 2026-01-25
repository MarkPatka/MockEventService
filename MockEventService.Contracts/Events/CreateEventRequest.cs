using MockEventService.Contracts.DTO;

namespace MockEventService.Contracts.Events;

public sealed record CreateEventRequest(
    string Title,
    Guid eventTypeId, 
    string eventTypeName,
    string? eventTypeIcon,
    string? eventTypeDescription, 
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    Guid OrganizerId,
    string? Description,
    LocationFullDto location);



