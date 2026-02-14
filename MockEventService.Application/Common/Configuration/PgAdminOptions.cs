namespace MockEventService.Application.Common.Configuration;

public class PgAdminOptions
{
    public const string SectionName = nameof(PgAdminOptions);

    public string DefaultEmail    { get; set; } = null!;
    public string DefaultPassword { get; set; } = null!;

    public int Port { get; set; }



}
