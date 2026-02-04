using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain.EventAggregate.DomainEvents;

public sealed record EventCancelled(EventId EventId, DateTime CancelledAt) : IDomainEvent
{
    public DateTime OccurredOn => CancelledAt;
}
