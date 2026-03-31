namespace IMS.Jobs.Interfaces;

public interface INotificationService
{
    Task SendEscalationNotificationAsync(int incidentId, string message);
}
