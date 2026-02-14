namespace MockEventService.Application.EventSourcing;

/// <summary>
/// Base class for integration events sent to external systems
/// Uses primitive types (Guid instead of EntityId) for serialization
/// </summary>
public abstract record IntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public string EventType { get; init; } = string.Empty;
}
