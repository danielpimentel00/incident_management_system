using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IMS.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
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

        _logger.LogInformation("Handling {RequestName}", requestName);
        
        try
        {
            var response = await next();
            
            stopWatch.Stop();
            _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", requestName, stopWatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopWatch.Stop();
            _logger.LogError(ex, "Error handling {RequestName} after {ElapsedMilliseconds}ms", requestName, stopWatch.ElapsedMilliseconds);
            throw;
        }
    }
}
