using IMS.Jobs.Events;
using IMS.Jobs.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace IMS.Jobs.Services;

public class IncidentEscalationConsumer : BackgroundService
{
    private readonly INotificationService _notificationService;

    public IncidentEscalationConsumer(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        string exchange = "incident-exchange";

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout);

        var queueName = (await channel.QueueDeclareAsync()).QueueName;

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchange,
            routingKey: ""
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var evt = JsonSerializer.Deserialize<IncidentEscalatedEvent>(json);

            await HandleEvent(evt);
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }

    private async Task HandleEvent(IncidentEscalatedEvent? evt)
    {
        if (evt == null) return;

        await _notificationService.SendEscalationNotificationAsync(evt.IncidentId, "Incident Escalated");
    }
}
