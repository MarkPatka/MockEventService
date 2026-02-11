using System.Text.Json;
using Microsoft.Extensions.Logging;
using UserService.Application.IntegrationEvents;
using UserService.Infrastructure.Persistence.Outbox;

namespace UserService.Infrastructure.Persistence;

public sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly UserServiceDbContext _context;
    private readonly ILogger<IntegrationEventPublisher> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public IntegrationEventPublisher(
        UserServiceDbContext context,
        ILogger<IntegrationEventPublisher> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        var type = integrationEvent.GetType().FullName!;

        var payload = JsonSerializer.Serialize(integrationEvent, _jsonOptions);

        var outboxMessage = new OutboxMessage(
            integrationEvent,
            payload,
            type);

        await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);

        _logger.LogDebug(
            "Integration event {EventType} added to outbox with id {EventId}",
            type,
            integrationEvent.Id);
    }
}