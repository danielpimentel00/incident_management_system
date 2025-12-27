namespace incident_management_system.API.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestTime = DateTime.UtcNow;
        
        Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");
        await _next(context);
        Console.WriteLine($"Outgoing Response: {context.Response.StatusCode}");
        
        var responseTime = DateTime.UtcNow;
        Console.WriteLine($"Request Time: {responseTime - requestTime}");
    }
}
