using IMS.Jobs.Interfaces;
using System.Diagnostics;
using System.Net.Http.Json;

namespace IMS.Jobs.ExternalServices;

public class HttpBinNotificationService : INotificationService
{
    private readonly HttpClient _client;
    private readonly ILogger<HttpBinNotificationService> _logger;

    public HttpBinNotificationService(HttpClient client, ILogger<HttpBinNotificationService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task SendEscalationNotificationAsync(int incidentId, string message)
    {
        var stopwatch = Stopwatch.StartNew();

        string endpoint = message switch
        {
            "Force 500" => "status/500",
            "Force delay" => "delay/10",
            _ => "post"
        };

        var response = await _client.PostAsJsonAsync(endpoint, new
        {
            IncidentId = incidentId,
            Message = message
        });

        stopwatch.Stop();

        _logger.LogInformation(
            "HttpBin call took {Elapsed} ms. Status: {StatusCode}",
            stopwatch.ElapsedMilliseconds,
            response.StatusCode);

        response.EnsureSuccessStatusCode();
    }
}
