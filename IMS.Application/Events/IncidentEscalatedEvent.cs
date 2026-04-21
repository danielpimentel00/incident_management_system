namespace IMS.Application.Events;

public record IncidentEscalatedEvent(
    int IncidentId,
    string EscalationLevel,
    DateTime EscalatedAt
);
