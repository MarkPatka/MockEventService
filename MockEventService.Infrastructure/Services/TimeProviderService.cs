using MockEventService.Application.Services;

namespace MockEventService.Infrastructure.Services;

public class TimeProviderService 
    : ITimeProviderService
{
    public DateTime Now => TimeProvider.System.GetLocalNow().DateTime;
    public DateTime UtcNow => TimeProvider.System.GetUtcNow().DateTime;
}
