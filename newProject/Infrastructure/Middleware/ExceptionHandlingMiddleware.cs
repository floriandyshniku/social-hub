using System.Net;
using System.Text.Json;
using newProject.Domain.Users.Exceptions;

namespace newProject.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            // Domain exceptions
            UserNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            UserAlreadyFollowingException => (HttpStatusCode.BadRequest, exception.Message),
            UserCannotFollowSelfException => (HttpStatusCode.BadRequest, exception.Message),
            UserInactiveException => (HttpStatusCode.BadRequest, exception.Message),
            
            // Validation exceptions (more specific first)
            ArgumentNullException => (HttpStatusCode.BadRequest, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            
            // Business logic exceptions
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            
            // Default for unexpected exceptions
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        // Log the exception
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        // Create error response
        var errorResponse = new
        {
            Error = message,
            StatusCode = (int)statusCode,
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier
        };

        response.StatusCode = (int)statusCode;
        
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await response.WriteAsync(jsonResponse);
    }
} 