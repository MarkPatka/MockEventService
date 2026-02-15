using Microsoft.Extensions.Options;
using MockEventService.Api;
using MockEventService.Application;
using MockEventService.Application.Common.Configuration;
using MockEventService.Infrastructure;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation(builder.Configuration)
        .AddInfrastructure(builder.Configuration)
        .AddApplication()
        ;
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.MapControllers();
    app.Run();
}


