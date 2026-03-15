using System.Net.Http.Json;

namespace IMS.Jobs.Services;

public class ApiHealthCheckService : BackgroundService
{
    private readonly ILogger<ApiHealthCheckService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly TimeSpan _checkInterval;

    public ApiHealthCheckService(
        ILogger<ApiHealthCheckService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _checkInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("HealthCheck:IntervalSeconds", 30));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("API Health Check Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckApiHealth(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking API health");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("API Health Check Service stopped");
    }

    private async Task CheckApiHealth(CancellationToken cancellationToken)
    {
        var apiUrl = _configuration.GetValue<string>("HealthCheck:ApiUrl");
        
        if (string.IsNullOrEmpty(apiUrl))
        {
            _logger.LogWarning("API URL not configured for health checks");
            return;
        }

        var httpClient = _httpClientFactory.CreateClient("ApiHealthCheck");

        try
        {
            var response = await httpClient.GetAsync($"{apiUrl}/api/health", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var healthStatus = await response.Content.ReadFromJsonAsync<HealthStatusResponse>(cancellationToken: cancellationToken);
                
                _logger.LogInformation(
                    "API Health Check: {Status} - Service: {Service} - Timestamp: {Timestamp}",
                    healthStatus?.Status,
                    healthStatus?.Service,
                    healthStatus?.Timestamp);
            }
            else
            {
                _logger.LogWarning(
                    "API Health Check failed with status code: {StatusCode}",
                    response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to API at {ApiUrl}", apiUrl);
        }
        catch (TaskCanceledException ex)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "API Health Check timed out");
            }
        }
    }

    private record HealthStatusResponse(string Status, DateTime Timestamp, string Service);
}
