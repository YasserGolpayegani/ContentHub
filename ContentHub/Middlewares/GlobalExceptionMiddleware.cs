using ContentHub.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
            _logger.LogError(exception, "Unhandled exception occurred.");

            await HandleExceptionAsync(context, exception);
        }
    }

    public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),

            ValidationException => (StatusCodes.Status400BadRequest, "Bad Request"),
            
            IdentityOperationException => (StatusCodes.Status400BadRequest, "Identity Operation Failed"),

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

        if (exception is ValidationException validationException)
            problemDetails.Extensions["errors"] = validationException.Errors;

        if (exception is IdentityOperationException identityException)
        {
            problemDetails.Extensions["errors"] = identityException.Errors
                .Select(error => new
                {
                    error.Code,
                    error.Description
                });
        }

        await context.Response.WriteAsJsonAsync(problemDetails);

        
    }

}
