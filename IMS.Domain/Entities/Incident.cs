using IMS.Domain.Enums;

namespace IMS.Domain.Entities;

public class Incident
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public ICollection<IncidentComment> Comments { get; set; } = [];

    public void MarkAsResolved()
    {
        Status = IncidentStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(IncidentStatus newStatus)
    {
        // rule: Cannot reopen a resolved incident
        if (Status == IncidentStatus.Resolved && newStatus == IncidentStatus.Open)
            throw new InvalidOperationException(
                "Cannot reopen a resolved incident. Create a new incident instead.");

        if (Status == newStatus)
            return;

        if (newStatus == IncidentStatus.Resolved)
        {
            MarkAsResolved();
        }
        else
        {
            Status = newStatus;
        }
    }
}
