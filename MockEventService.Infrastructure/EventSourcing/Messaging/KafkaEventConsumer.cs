using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockEventService.Application.Common.Configuration;
using MockEventService.Application.EventSourcing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockEventService.Infrastructure.EventSourcing.Messaging;

public class KafkaEventConsumer : BackgroundService, IEventConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaEventConsumer> _logger;
    private readonly IDeadLetterQueueProducer _dlqProducer;
    private readonly KafkaOptions _options;

    private IConsumer<string, string>? _consumer;

    public KafkaEventConsumer(
        IServiceScopeFactory scopeFactory,
        IOptions<KafkaOptions> options,
        ILogger<KafkaEventConsumer> logger,
        IDeadLetterQueueProducer dlqProducer)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
        _dlqProducer = dlqProducer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _consumer = BuildConsumer();
        _consumer.Subscribe(_options.ConsumeEventsTopics);

        _logger.LogInformation(
            "Kafka consumer started. Group: {group}, Topics: [{topics}]",
            _options.ConsumerGroupId, string.Join(", ", _options.ConsumeEventsTopics));

        try
        {
            await ConsumeMessagesAsync(stoppingToken);
        }
        finally
        {
            _consumer?.Close();
            _consumer?.Dispose();
            _logger.LogInformation("Kafka consumer stopped.");
        }
    }

    public async Task ConsumeMessagesAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer!.Consume(stoppingToken);

                if (consumeResult?.IsPartitionEOF == true)
                    continue;

                if (consumeResult?.Message?.Value == null)
                {
                    _logger.LogWarning("Received null message, committing offset");
                    _consumer.Commit(consumeResult);
                    continue;
                }

                _logger.LogInformation(
                    "Received message from topic '{topic}' [partition {partition}, offset {offset}], key: {key}",
                    consumeResult.Topic, consumeResult.Partition.Value,
                    consumeResult.Offset.Value, consumeResult.Message.Key);

                var success = await TryProcessMessageWithRetryAsync(consumeResult, stoppingToken);

                if (success)
                {
                    _consumer.Commit(consumeResult);
                }
            }
            catch (ConsumeException ex) when (ex.Error.IsFatal)
            {
                _logger.LogError(ex, "Fatal Kafka consume error: {Reason}", ex.Error.Reason);
                throw;
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                await Task.Delay(1000, stoppingToken); 
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Kafka consumer received shutdown signal.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in consumer loop");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
    
    
    private IConsumer<string, string> BuildConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.ConsumerGroupId,
            EnableAutoCommit = false,
            SessionTimeoutMs = 10000,
            HeartbeatIntervalMs = 3000,
            MaxPollIntervalMs = 300000,

            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(
                _options.AutoOffsetReset, true),
        };

        return new ConsumerBuilder<string, string>(config)
            .SetErrorHandler((_, e) =>
                _logger.LogError("Kafka consumer error: {Reason}", e.Reason))
            .SetPartitionsAssignedHandler((_, partitions) =>
                _logger.LogDebug("Assigned partitions: {Partitions}", string.Join(", ", partitions)))
            .SetPartitionsRevokedHandler((_, partitions) =>
                _logger.LogDebug("Revoked partitions: {Partitions}", string.Join(", ", partitions)))
            .Build();
    }


    private async Task<bool> TryProcessMessageWithRetryAsync(
        ConsumeResult<string, string> consumeResult,
        CancellationToken ct)
    {
        Exception? lastException = null;

        for (int attempt = 1; attempt <= _options.MaxRetryAttempts; attempt++)
        {
            try
            {
                await ProcessMessageAsync(consumeResult, ct);
                return true;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                lastException = ex;

                _logger.LogWarning(ex,
                    "Attempt {attempt}/{maxAttempts} failed for message on topic '{topic}' [offset {offset}]",
                    attempt, _options.MaxRetryAttempts,
                    consumeResult.Topic, consumeResult.Offset.Value);

                if (attempt < _options.MaxRetryAttempts)
                {
                    var delay = _options.RetryDelayMs * attempt;
                    await Task.Delay(delay, ct);
                }
            }
        }

        _logger.LogError(
            "Message on topic '{topic}' [offset {offset}] failed after {maxAttempts} attempts. Sending to DLQ.",
            consumeResult.Topic, consumeResult.Offset.Value, _options.MaxRetryAttempts);

        await _dlqProducer.PublishAsync(
            consumeResult.Topic,
            consumeResult.Message?.Key,
            consumeResult.Message?.Value,
            $"Failed after {_options.MaxRetryAttempts} retry attempts",
            lastException,
            ct);

        try
        {
            _consumer!.Commit(consumeResult);
        }
        catch (KafkaException commitEx)
        {
            _logger.LogError(commitEx, "Failed to commit offset after DLQ publish");
        }

        return false;
    }

    private async Task ProcessMessageAsync(
        ConsumeResult<string, string> consumeResult,
        CancellationToken cancellationToken)
    {
        var envelope = JsonSerializer.Deserialize<KafkaEventEnvelope>(
            consumeResult.Message.Value,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (envelope == null)
        {
            throw new InvalidOperationException("Failed to deserialize message envelope");
        }

        _logger.LogInformation(
            "Processing event: Type={type}, EventId={id}",
            envelope.EventType, envelope.EventId);

        // Create scope for each message to ensure proper isolation
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        // Map envelope to command based on EventType
        var command = MapToCommand(envelope);
        await mediator.Send(command, cancellationToken);
    }

    private IRequest MapToCommand(KafkaEventEnvelope envelope)
    {
        throw new NotImplementedException("Implement mapping from envelope to command");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka consumer stopping...");
        await base.StopAsync(cancellationToken);
    }

}

    // пример
    public class KafkaEventEnvelope
{
    [JsonPropertyName("EventId")]
    public string EventId { get; set; }

    [JsonPropertyName("EventType")]
    public string EventType { get; set; }

    [JsonPropertyName("EventName")]
    public string EventName { get; set; }

    [JsonPropertyName("Timestamp")]
    public string Timestamp { get; set; }

    /// <summary>
    /// Это может быть Event, User, Club ... 
    /// Payload - это также то, что будет передано в качестве параметров command или query.
    /// Для обработки пришедшего эвента нам потребуется соответствующая команда или запрос. Как их получить?
    /// Для этого мы можем воспользоваться полем EventType, после чего при парсинге данного сообщения вызывать
    /// некий класс-маппер, который будет по названию евента возвращать определенный command/query
    /// </summary>
    [JsonPropertyName("Payload")]
    public JsonElement? Payload { get; set; }
}