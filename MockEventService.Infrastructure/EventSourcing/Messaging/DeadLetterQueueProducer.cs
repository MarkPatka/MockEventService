using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockEventService.Application.Common.Configuration;
using MockEventService.Application.EventSourcing;
using System.Text.Json;

namespace MockEventService.Infrastructure.EventSourcing.Messaging;

public class DeadLetterQueueProducer : IDeadLetterQueueProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<DeadLetterQueueProducer> _logger;
    private readonly string _dlqTopic;

    public DeadLetterQueueProducer(
        ILogger<DeadLetterQueueProducer> logger,
        IOptions<KafkaOptions> settings)
    {
        _logger = logger;
        _dlqTopic = settings.Value.DeadLetterTopic;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }


    public async Task PublishAsync(
        string originalTopic,
        string? messageKey,
        string? messageValue,
        string errorReason,
        Exception? exception = null,
        CancellationToken ct = default)
    {
        try
        {
            var dlqMessage = new
            {
                OriginalTopic = originalTopic,
                OriginalKey = messageKey,
                OriginalValue = messageValue,
                Error = errorReason,
                ExceptionType = exception?.GetType().Name,
                ExceptionMessage = exception?.Message,
                StackTrace = exception?.StackTrace,
                Timestamp = DateTimeOffset.UtcNow
            };

            var result = await _producer.ProduceAsync(_dlqTopic,
                new Message<string, string>
                {
                    Key = messageKey ?? "unknown",
                    Value = JsonSerializer.Serialize(dlqMessage)
                }, ct);

            _logger.LogWarning(
                "Message sent to DLQ topic '{topic}' (partition {partition}, offset {offset}). Reason: {reason}",
                _dlqTopic, result.Partition.Value, result.Offset.Value, errorReason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "CRITICAL: Failed to publish to DLQ topic '{topic}'. Original error: {error}",
                _dlqTopic, errorReason);
        }
    }


    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(5));
        _producer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
