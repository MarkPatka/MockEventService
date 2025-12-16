using MockEventService.Api.Middleware.GlobalErrorHandler;

namespace MockEventService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddErrorHandler();
        return services;
    }

    private static IServiceCollection AddErrorHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
