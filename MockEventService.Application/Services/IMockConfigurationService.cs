using MockEventService.Application.Common.Configuration;

namespace MockEventService.Application.Services;

public interface IMockConfigurationService
{
    public EventsDatabaseConnection GetDatabaseInfo();
    public ApiSettings GetApiSettings();
    public PgAdminSettings GetPgAdminSettings();
}
