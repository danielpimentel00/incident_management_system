using Grpc.Core;
using IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;
using IMS.Application.Features.Incidents.Queries.GetOpenIncidents;
using IMS.GrpcService.Protos;
using MediatR;

namespace IMS.GrpcService.Services;

public class IncidentGrpcService : IncidentService.IncidentServiceBase
{
    private readonly IMediator _mediator;

    public IncidentGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<OpenIncidentsResponse> GetOpenIncidents(EmptyRequest request, ServerCallContext context)
    {
        var query = new GetOpenIncidentsQuery();
        var result = await _mediator.Send(query);

        var response = new OpenIncidentsResponse();

        foreach (var incident in result.Incidents)
        {
            response.Incidents.Add(new Incident
            {
                Id = incident.Id,
                Title = incident.Title,
                OpenedAtUnix = new DateTimeOffset(incident.CreatedAt).ToUnixTimeSeconds()
            });
        }

        return response;
    }

    public override async Task<EscalateIncidentResponse> EscalateIncident(EscalateIncidentRequest request, ServerCallContext context)
    {
        var cmd = new UpdateIncidentStatusCommand
        {
            Id = request.IncidentId,
            Status = Domain.Enums.IncidentStatus.Escalated
        };
        var success = await _mediator.Send(cmd);

        var response = new EscalateIncidentResponse { Success = success };

        return response;
    }
}
