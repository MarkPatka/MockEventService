using MockEventService.Api;
using MockEventService.Application;
using MockEventService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation(builder.Configuration)
        .AddInfrastructure()
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


