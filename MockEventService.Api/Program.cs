using MockEventService.Api;
using MockEventService.Application;
using MockEventService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure()
        .AddApplication()
        .AddPresentation()
        ;
}


var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.MapControllers();
    app.Run();
}


