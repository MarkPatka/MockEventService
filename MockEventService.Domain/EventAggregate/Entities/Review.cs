using Microsoft.Extensions.Logging;
using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain.EventAggregate.Entities;

public sealed class Review : Entity<ReviewId>
{
    public ValueObjects.EventId EventId { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    private Review() { }

    public Review(ReviewId id) : base(id) { }
}