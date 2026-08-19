using Serilog.Context;
using System.Security.Claims;

namespace ContentHub.Api.Middlewares;

public sealed class LoggingContextMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var userId = user.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        var roles = user.FindAll(
                ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToArray();

        var properties = new List<IDisposable>();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            properties.Add(
                LogContext.PushProperty(
                    "UserId",
                    userId));
        }

        if (roles.Length > 0)
        {
            properties.Add(
                LogContext.PushProperty(
                    "Roles",
                    roles));
        }

        try
        {
            await _next(context);
        }
        finally
        {
            foreach (var property in properties)
            {
                property.Dispose();
            }
        }
    }
}