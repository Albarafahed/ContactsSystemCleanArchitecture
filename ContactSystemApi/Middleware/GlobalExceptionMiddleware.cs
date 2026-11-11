
using System.ComponentModel.DataAnnotations;
using ContactsSystemApplication.Exceptions;
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
            _logger.LogDebug(
                "Request was cancelled by the client. TraceId: {TraceId}",
                context.TraceIdentifier);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
                throw;

            var (statusCode, title) = exception switch
            {
                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound,
                     "Resource not found"),

                ForbiddenAccessException =>
                    (StatusCodes.Status403Forbidden,
                     "Access denied"),

                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized,
                     "Authentication required"),

                ValidationException =>
                    (StatusCodes.Status400BadRequest,
                     "Validation failed"),

                ArgumentException =>
                    (StatusCodes.Status400BadRequest,
                     "Invalid request"),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                     "An unexpected error occurred")
            };

            if (statusCode >= 500)
            {
                _logger.LogError(
                    exception,
                    "Unhandled server error. TraceId: {TraceId}",
                    context.TraceIdentifier);
            }
            else
            {
                _logger.LogWarning(
                    "Request failed with status {StatusCode}. " +
                    "Exception: {ExceptionType}. TraceId: {TraceId}",
                    statusCode,
                    exception.GetType().Name,
                    context.TraceIdentifier);
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = statusCode >= 500
                        ? "Please try again later."
                        : exception.Message,
                    Instance = context.Request.Path,
                    Extensions =
                    {
                        ["traceId"] = context.TraceIdentifier
                    }
                },
                context.RequestAborted);
        }
    }
}
