using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.Enumerations;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain.EventAggregate.Entities;

public sealed class Participant : Entity<ParticipantId>
{
    public EventId EventId { get; private set; } = null!;
    public UserId UserId   { get; private set; } = null!;

    public string UserName          { get; private set; } = string.Empty; 
    public DateTime RegisteredAt    { get; private set; }
    public ParticipantStatus Status { get; private set; } = null!;

    private Participant() { }

    private Participant(
        ParticipantId id,
        EventId eventId,
        UserId userId,
        string userName,
        DateTime registeredAt)
        : base(id)
    {
        EventId = eventId;
        UserId = userId;
        UserName = userName;
        RegisteredAt = registeredAt;
        Status = ParticipantStatus.Registered;
    }

    public static Participant Create(EventId eventId, UserId userId, string userName, DateTime registeredAt)
    {
        return new Participant(ParticipantId.CreateUnique(), eventId, userId, userName, registeredAt);
    }

    public void MarkAsAttended()
    {
        Status = ParticipantStatus.Attended;
    }

    public void Cancel()
    {
        Status = ParticipantStatus.Cancelled;
    }
}
