using IMS.GrpcService.Protos;

namespace IMS.Jobs.Services;

public class IncidentEscalationService : BackgroundService
{
    private readonly ILogger<IncidentEscalationService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
    private readonly TimeSpan _escalationThreshold = TimeSpan.FromHours(24);

    public IncidentEscalationService(
        ILogger<IncidentEscalationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Incident Escalation Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndEscalateIncidents(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking incidents for escalation");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Incident Escalation Service stopped");
    }

    private async Task CheckAndEscalateIncidents(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var grpcClient = scope.ServiceProvider.GetRequiredService<IncidentService.IncidentServiceClient>();

        _logger.LogDebug("Checking for incidents to escalate");

        var response = await grpcClient.GetOpenIncidentsAsync(
            new EmptyRequest(),
            cancellationToken: cancellationToken);

        var currentTime = DateTimeOffset.UtcNow;
        var incidentsToEscalate = response.Incidents
            .Where(incident =>
            {
                var openedAt = DateTimeOffset.FromUnixTimeSeconds(incident.OpenedAtUnix);
                var age = currentTime - openedAt;
                return age >= _escalationThreshold;
            })
            .ToList();

        if (incidentsToEscalate.Count == 0)
        {
            _logger.LogDebug("No incidents require escalation");
            return;
        }

        _logger.LogInformation("Found {Count} incidents requiring escalation", incidentsToEscalate.Count);

        foreach (var incident in incidentsToEscalate)
        {
            try
            {
                var escalateResponse = await grpcClient.EscalateIncidentAsync(
                    new EscalateIncidentRequest { IncidentId = incident.Id },
                    cancellationToken: cancellationToken);

                if (escalateResponse.Success)
                {
                    _logger.LogInformation("Successfully escalated incident {IncidentId}: {Title}",
                        incident.Id, incident.Title);
                }
                else
                {
                    _logger.LogWarning("Failed to escalate incident {IncidentId}: {Title}",
                        incident.Id, incident.Title);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error escalating incident {IncidentId}", incident.Id);
            }
        }
    }
}
