using MockEventService.Domain.EventAggregate.Entities;
using MockEventService.Domain.EventAggregate.Enumerations;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.EventManagement.Common;

public record CreateEventResult(
        Guid EventId,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        int StatusId
    );
