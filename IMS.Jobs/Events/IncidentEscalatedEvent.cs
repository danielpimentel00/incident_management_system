namespace IMS.Jobs.Events;

public record IncidentEscalatedEvent(
    int IncidentId,
    string EscalationLevel,
    DateTime EscalatedAt
);
