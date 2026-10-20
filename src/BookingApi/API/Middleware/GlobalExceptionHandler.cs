using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookingApi.API.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation Error", exception.Message),
            ArgumentException or InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", "You are not authorized to perform this action."),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Conflict", "The resource was modified by another request. Please retry."),
            DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } } => (StatusCodes.Status409Conflict, "Conflict", "A resource with the same unique value already exists."),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "An unhandled exception occurred.");
        }
        else
        {
            logger.LogWarning("An exception occurred ({Status}): {Message}", statusCode, exception.Message);
        }

        var problemDeatails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDeatails, cancellationToken);

        return true;

    }
}