using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain.EventAggregate.DomainEvents;

public sealed record EventCreated(EventId EventId, DateTime CreatedAt) : IDomainEvent
{
    public DateTime OccurredOn => CreatedAt;
}
