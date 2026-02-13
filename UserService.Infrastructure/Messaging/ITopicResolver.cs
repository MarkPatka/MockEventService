using UserService.Application.IntegrationEvents;

namespace UserService.Infrastructure.Messaging;

public interface ITopicResolver
{
    string Resolve(IIntegrationEvent integrationEvent);
}