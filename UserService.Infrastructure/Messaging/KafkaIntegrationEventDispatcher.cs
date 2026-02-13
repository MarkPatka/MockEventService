using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using UserService.Application.IntegrationEvents;

namespace UserService.Infrastructure.Messaging;

public class KafkaIntegrationEventDispatcher : IIntegrationEventDispatcher, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ITopicResolver _topicResolver;
    private readonly ILogger<KafkaIntegrationEventDispatcher> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public KafkaIntegrationEventDispatcher(
        ITopicResolver topicResolver,
        ILogger<KafkaIntegrationEventDispatcher> logger)
    {
        _topicResolver = topicResolver;
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            Acks = Acks.All,
            EnableIdempotence = true
        };

        _producer = new ProducerBuilder<string, string>(config)
            .Build();
    }

    public async Task DispatchAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        var topic = _topicResolver.Resolve(integrationEvent);
        var payload = JsonSerializer.Serialize(integrationEvent);
        var message = new Message<string, string>
        {
            Key = integrationEvent.GetType().Name,
            Value = payload
        };

        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public async Task DispatchToDeadLetterAsync(
        IIntegrationEvent integrationEvent,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var topic = _topicResolver.Resolve(integrationEvent);
        var dlqTopic = $"{topic}.dlq";

        var deadLetter = new DeadLetterMessage
        {
            OriginalMessageId = integrationEvent.Id,
            OriginalType = integrationEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(
                integrationEvent,
                integrationEvent.GetType(),
                _serializerOptions),
            Error = exception.Message,
            StackTrace = exception.ToString(),
            FailedAtUtc = DateTime.UtcNow
        };

        var payload = JsonSerializer.Serialize(deadLetter, _serializerOptions);

        await _producer.ProduceAsync(
            dlqTopic,
            new Message<string, string>
            {
                Key = integrationEvent.Id.ToString(),
                Value = payload,
                Headers = new Headers
                {
                    { "x-dead-letter", Encoding.UTF8.GetBytes("true") },
                    { "x-error-type", Encoding.UTF8.GetBytes(exception.GetType().Name) }
                }
            },
            cancellationToken);
    }


    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}