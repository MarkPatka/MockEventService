using UserService.Api;
using UserService.Application;
using UserService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddMappings()
        .AddApplication()
        .AddInfrastructure()
        .AddControllers();

    // IOptions<T>, Configuration classes 
    // ErrorOr<Result>
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



