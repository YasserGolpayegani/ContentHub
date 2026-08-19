using ContentHub.Api.Middlewares;

namespace ContentHub.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseLoggingContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<LoggingContextMiddleware>();
        return app;
    }

}
