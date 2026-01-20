using MediatR;
using System.Diagnostics;

namespace incident_management_system.API.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var stopWatch = Stopwatch.StartNew();
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {0}", requestName);
        
        try
        {
            var response = await next(cancellationToken);
            
            stopWatch.Stop();
            _logger.LogInformation("Handled {0} in {1}ms", requestName, stopWatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopWatch.Stop();
            _logger.LogError(ex, "Error handling {0} after {1}ms", requestName, stopWatch.ElapsedMilliseconds);
            throw;
        }
    }
}