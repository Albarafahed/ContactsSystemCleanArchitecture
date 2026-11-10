
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
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
        catch (OperationCanceledException)
            when (context.RequestAborted.IsCancellationRequested)
        {
            // The client disconnected or cancelled the request.
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred.");

            if (context.Response.HasStarted)
                throw;

            var (statusCode, title) = exception switch
            {
                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound, "Resource not found"),

                UnauthorizedAccessException =>
                    (StatusCodes.Status403Forbidden, "Access denied"),

                ValidationException =>
                    (StatusCodes.Status400BadRequest, "Validation failed"),

                ArgumentException =>
                    (StatusCodes.Status400BadRequest, "Invalid request"),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                     "An unexpected error occurred")
            };

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = statusCode == 500
                        ? "Please try again later."
                        : exception.Message,
                    Instance = context.Request.Path
                });
        }
    }
}
