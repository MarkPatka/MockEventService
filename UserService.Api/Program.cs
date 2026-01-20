using UserService.Api;
using UserService.Application;
using UserService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure()
        ;

    // IOptions<T>, Configuration classes 
    // ErrorOr<Result>
}


var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.MapControllers();
    app.Run();
}



