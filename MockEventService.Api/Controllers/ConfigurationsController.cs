using Microsoft.AspNetCore.Mvc;
using MockEventService.Application.Services;
using MockEventService.Contracts.Events;

namespace MockEventService.Api.Controllers;

public class ConfigurationsController : ControllerBase
{
    private readonly IMockConfigurationService _configurationService;

    public ConfigurationsController(IMockConfigurationService configurationService) =>
        _configurationService = configurationService;

    [HttpGet("Show")]
    public async Task<IActionResult> ShowConfiguration()
    {
        var dbConfig      = _configurationService.GetDatabaseInfo();
        var pgAdminConfig = _configurationService.GetPgAdminSettings();
        var apiConfig     = _configurationService.GetApiSettings();

        return Ok();
    }
}
