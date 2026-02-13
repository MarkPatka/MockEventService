using UserService.Application.IntegrationEvents;
using UserService.Application.IntegrationEvents.Clubs;

namespace UserService.Infrastructure.Messaging;

public class TopicResolver : ITopicResolver
{
    public string Resolve(IIntegrationEvent integrationEvent)
    {
        return integrationEvent switch
        {
            ClubCreatedIntegrationEvent => "clubs-events",
            _ => throw new InvalidOperationException(
                $"No topic mapping for {integrationEvent.GetType().Name}")
        };
    }
}