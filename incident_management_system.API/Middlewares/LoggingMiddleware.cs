namespace incident_management_system.API.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestTime = DateTime.UtcNow;
        
        _logger.LogInformation("Incoming Request: {method} {path}", context.Request.Method, context.Request.Path);
        await _next(context);
        _logger.LogInformation("Outgoing Response: {statusCode}", context.Response.StatusCode);
        
        var responseTime = DateTime.UtcNow;
        var totalTime = responseTime - requestTime;
        _logger.LogInformation("Request Time: {requestTime}", totalTime);
    }
}
