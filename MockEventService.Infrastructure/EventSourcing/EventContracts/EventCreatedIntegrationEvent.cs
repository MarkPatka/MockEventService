using MockEventService.Application.EventSourcing;

namespace MockEventService.Infrastructure.EventSourcing.EventContracts;

/// <summary>
/// Integration event for event creation (sent to Kafka)
/// Converted from EventCreated domain event
/// </summary>
public sealed record EventCreatedIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }
    public DateTime CreatedAt { get; init; }

    public EventCreatedIntegrationEvent()
    {
        EventType = nameof(EventCreatedIntegrationEvent);
    }
}
