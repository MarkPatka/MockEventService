using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.IntegrationEvents;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}