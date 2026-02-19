using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.DomainEvents;
using MockEventService.Domain.EventAggregate.Entities;
using MockEventService.Domain.EventAggregate.Enumerations;
using MockEventService.Domain.EventAggregate.ValueObjects;
using EventId = MockEventService.Domain.EventAggregate.ValueObjects.EventId;

namespace MockEventService.Domain.EventAggregate;

public sealed class Event : AggregateRoot<EventId>
{
    private readonly List<Participant> _participants = [];
    public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();

    public string Title        { get; private set; } = string.Empty;
    public string Description  { get; private set; } = string.Empty;
    public EventType EventType { get; private set; } = null!;
    public Location Location   { get; private set; } = null!;
    public DateTime StartDate  { get; private set; }
    public DateTime EndDate    { get; private set; }
    public int MaxParticipants { get; private set; }
    public EventStatus Status  { get; private set; } = EventStatus.Draft;
    
    public UserId OrganizerId   { get; private set; } = null!;
    public string OrganizerName { get; private set; } = string.Empty;
    
    public DateTime CreatedAt  { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public int ReviewsCount       { get; private set; }
    public decimal? AverageRating { get; private set; } = null;

    private Event() { }

    private Event(
        EventId id, 
        string title, 
        string description,
        EventType eventType,
        Location location,
        DateTime startDate,
        DateTime endDate,
        int maxParticipants,
        UserId organizerId,
        DateTime createdAt,
        DateTime updatedAt,
        int reviewsCount = 0,
        decimal? averageRating = null) 
        : base(id)
    {
        Title = title;
        Description = description;
        EventType = eventType;
        Location = location;
        StartDate = startDate;
        EndDate = endDate;
        MaxParticipants = maxParticipants;
        OrganizerId = organizerId;
        ReviewsCount = reviewsCount;
        AverageRating = averageRating;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    // ON EVENT CREATED
    public static Event Create(
        string title,
        string description,
        EventType eventType,
        Location location,
        DateTime startDate,
        DateTime endDate,
        int maxParticipants,
        UserId organizerId,
        DateTime createdAt,
        DateTime updatedAt,
        int reviewsCount = 0,
        decimal? averageRating = null)
    {
        var @event = new Event(
            EventId.CreateUnique(),
            title,
            description,
            eventType,
            location,
            startDate,
            endDate,
            maxParticipants,
            organizerId,
            createdAt,
            updatedAt,
            reviewsCount,
            averageRating);

        @event.AddDomainEvent(new EventCreated(@event.Id, DateTime.UtcNow)); 
        return @event;
    }

    // ON EVENT PUBLISHED
    public void Publish() 
    {
        if (Status != EventStatus.Draft)
            throw new InvalidOperationException("Only draft events can be published");

        Status = EventStatus.Active;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new EventPublished(Id, DateTime.UtcNow));
    }

    // ON PARTICIPANT REGISTERED
    public Participant RegisterParticipant(UserId userId, string userName)
    {
        if (Status != EventStatus.Active)
            throw new InvalidOperationException("Only active events accept registrations");
        
        if (_participants.Count >= MaxParticipants)
            throw new InvalidOperationException("Event has reached maximum participants");

        var participant = Participant
            .Create(Id, userId, userName, DateTime.UtcNow);

        _participants.Add(participant);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ParticipantRegistered(
            Id, participant.Id, participant.RegisteredAt));

        return participant;
    }

    // ON EVENT CANCELLED
    public void Cancel()
    {
        if (Status != EventStatus.Active)
            throw new InvalidOperationException("Only active events can be cancelled");

        Status = EventStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new EventCancelled(Id, DateTime.UtcNow));
    }

    public void UpdateReviewStats(int reviewsCount, decimal averageRating)
    {
        if (reviewsCount < 0)
            throw new ArgumentException("Reviews count cannot be negative", nameof(reviewsCount));

        if (averageRating < 0 || averageRating > 5)
            throw new ArgumentException("Average rating must be between 0 and 5", nameof(averageRating));

        ReviewsCount = reviewsCount;
        AverageRating = reviewsCount > 0 ? averageRating : null; 
        UpdatedAt = DateTime.UtcNow;
    }

    public string GetRatingDisplay() => AverageRating.HasValue
        ? $"{AverageRating.Value:F2}"
        : "No rating yet";
}