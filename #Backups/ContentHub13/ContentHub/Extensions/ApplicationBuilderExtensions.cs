using ContentHub.Api.Middlewares;

namespace ContentHub.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();

        return app;
    }
}
