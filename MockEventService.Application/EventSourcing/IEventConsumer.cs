using Confluent.Kafka;

namespace MockEventService.Application.EventSourcing;

/// <summary>
/// Abstraction for consuming integration events from a message broker (e.g. Kafka).
/// </summary>
public interface IEventConsumer : IDisposable
{
    /// <summary>
    /// Start consuming messages. Typically runs until cancellation is requested.
    /// </summary>
    Task ConsumeMessagesAsync(CancellationToken stoppingToken);
}
