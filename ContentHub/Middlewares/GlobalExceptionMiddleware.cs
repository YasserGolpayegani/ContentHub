using ContentHub.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            LogException(exception);

            await HandleExceptionAsync(context, exception);
        }
    }

    private void LogException(Exception exception)
    {
        switch (exception)
        {
            case UnauthorizedException:
                _logger.LogWarning(exception, "Authentication failed.");
                break;

            case ValidationException:
                _logger.LogWarning(exception, "Request validation failed.");
                break;

            case NotFoundException:
                _logger.LogWarning(exception, "Requested resource was not found.");
                break;

            default:
                _logger.LogError(exception, "An unhandled exception occurred.");
                break;
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),

            ValidationException => (StatusCodes.Status400BadRequest, "Bad Request"),

            NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}