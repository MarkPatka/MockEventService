using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain.EventAggregate.DomainEvents;

public sealed record EventPublished(EventId EventId, DateTime OccurredAt) : IDomainEvent
{
    public DateTime OccurredOn => OccurredAt;
}
