using Confluent.Kafka;

namespace MockEventService.Application.EventSourcing;

/// <summary>
/// Abstraction for consuming integration events from a message broker (e.g. Kafka).
/// </summary>
public interface IEventConsumer : IDisposable
{
    public void Initialize();

    /// <summary>
    /// Start consuming messages. Typically runs until cancellation is requested.
    /// </summary>
    public Task ConsumeMessagesAsync(CancellationToken stoppingToken);

    public void Close();
}
