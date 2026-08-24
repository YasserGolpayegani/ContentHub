using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace ContentHub.Api.HealthChecks
{
    public sealed class HealthCheckResponseWriter
    {
        public static async Task WriteResponseAsync(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration.TotalMicroseconds,
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    duration = entry.Value.Duration.TotalMicroseconds
                })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
