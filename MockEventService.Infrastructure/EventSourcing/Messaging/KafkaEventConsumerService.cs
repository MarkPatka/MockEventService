using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MockEventService.Application.EventSourcing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockEventService.Infrastructure.EventSourcing.Messaging;

public class KafkaEventConsumerService : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private readonly ILogger<KafkaEventConsumerService> _logger;

    public KafkaEventConsumerService(
        IEventConsumer consumer,
        ILogger<KafkaEventConsumerService> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield(); // Immediate Continuation Scheduling:  await Task.Run(() => _consumer.Initialize(), stoppingToken); 

        try
        {
            _consumer.Initialize();
            await _consumer.ConsumeMessagesAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer service is stopping.");
        }
        catch (ObjectDisposedException)
        {
            _logger.LogInformation("Kafka consumer was disposed during operation.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error in Kafka consumer service");
        }
        finally
        {
            _consumer.Close();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka consumer background service stopping...");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing KafkaConsumerBackgroundService...");
        base.Dispose();
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