using Microsoft.Extensions.Logging;
using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.Entities;
using MockEventService.Domain.EventAggregate.Enumerations;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain;

public sealed class Event : AggregateRoot<EventAggregate.ValueObjects.EventId>
{
    private readonly List<Review> _reviews = [];
    private readonly List<Participant> _participants = [];


    public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();


    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public EventType EventType { get; private set; } = null!;
    public Location Location { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int MaxParticipants { get; private set; }
    public EventStatus Status { get; private set; } = EventStatus.Draft;
    public OrganizerId OrganizerId { get; private set; } = null!;


    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Event() { }
    // public static Event Create() => new();

    //public Event(EventId id) : base(id) { }
}