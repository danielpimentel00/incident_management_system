using IMS.Domain.Enums;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncident;

public class UpdateIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
