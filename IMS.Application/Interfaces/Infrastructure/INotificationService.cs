namespace IMS.Application.Interfaces.Infrastructure;

public interface INotificationService
{
    Task SendEscalationNotificationAsync(int incidentId, string message);
}
