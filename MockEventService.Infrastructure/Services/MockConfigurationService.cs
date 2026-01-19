using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockEventService.Application.Common.Configuration;
using MockEventService.Application.Services;

namespace MockEventService.Infrastructure.Services;

internal class MockConfigurationService : IMockConfigurationService
{
    private readonly EventsDatabaseConnection _dbConfig;
    private readonly ApiSettings _apiSettings;
    private readonly PgAdminSettings _pgAdminSettings;

    private readonly ILogger<MockConfigurationService> _logger;


    // Three ways to inject configuration:
    // 1. IOptions<T>         - Singleton, configuration read once at startup
    // 2. IOptionsSnapshot<T> - Scoped, reloads configuration per request
    // 3. IOptionsMonitor<T>  - Singleton, reloads configuration on change
    public MockConfigurationService(
        IOptions<EventsDatabaseConnection> dbConfig,
        IOptions<ApiSettings> apiSettings,
        IOptionsSnapshot<PgAdminSettings> pgAdminSettings,
        ILogger<MockConfigurationService> logger)
    {
        _dbConfig = dbConfig.Value;
        _apiSettings = apiSettings.Value;
        _pgAdminSettings = pgAdminSettings.Value;

        _logger = logger;
    }

    public EventsDatabaseConnection GetDatabaseInfo()
    {
        _logger.LogInformation("Log proccess");
        _logger.LogWarning("Log smth strange");
        _logger.LogError("Errors");

        return _dbConfig;
    }
    public PgAdminSettings GetPgAdminSettings() => _pgAdminSettings;
    public ApiSettings GetApiSettings() => _apiSettings;
}
