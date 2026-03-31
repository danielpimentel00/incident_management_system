namespace IMS.Application.Interfaces.Infrastructure;

public interface IEventBus
{
    Task PublishAsync<T>(T @event);
}