using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.Common.Abstract;

namespace UserService.Domain.ClubAggregate.DomainEvents;

public sealed record ClubCreatedDomainEvent(ClubId ClubId, DateTime CreatedAt) : IDomainEvent
{
    public DateTime OccurredOn => CreatedAt;
}
