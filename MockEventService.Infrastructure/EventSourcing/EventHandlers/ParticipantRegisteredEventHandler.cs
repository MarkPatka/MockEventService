using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockEventService.Application.Common.Configuration;
using MockEventService.Application.EventSourcing;
using MockEventService.Domain.EventAggregate.DomainEvents;
using MockEventService.Infrastructure.EventSourcing.EventContracts;

namespace MockEventService.Infrastructure.EventSourcing.EventHandlers;

public sealed class ParticipantRegisteredDomainEventHandler : INotificationHandler<ParticipantRegistered>
{
    private readonly IEventProducer _eventProducer;
    private readonly KafkaOptions _options;
    private readonly ILogger<ParticipantRegisteredDomainEventHandler> _logger;

    public ParticipantRegisteredDomainEventHandler(
        IEventProducer eventProducer,
        IOptions<KafkaOptions> options,
        ILogger<ParticipantRegisteredDomainEventHandler> logger)
    {
        _eventProducer = eventProducer;
        _options = options.Value;
        _logger = logger;
    }

    public async Task Handle(ParticipantRegistered notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new ParticipantRegisteredIntegrationEvent
        {
            EventId = notification.EventId.Value,
            ParticipantId = notification.ParticipantId.Value,
            RegisteredAt = notification.RegisteredAt
        };

        await _eventProducer.ProduceAsync(
            _options.ProduceEventsTopic,
            $"{notification.EventId.Value}_{notification.ParticipantId.Value}",
            integrationEvent,
            cancellationToken);

        _logger.LogDebug(
            "Published ParticipantRegistered to Kafka for EventId {EventId}, ParticipantId {ParticipantId}",
            notification.EventId.Value,
            notification.ParticipantId.Value);
    }
}