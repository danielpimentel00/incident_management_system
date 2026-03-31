using IMS.Application.Interfaces.Infrastructure;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace IMS.Infrastructure.Services.MessageBroker;

public class RabbitMqEventBus : IEventBus
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    private RabbitMqEventBus(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public static async Task<RabbitMqEventBus> CreateAsync(string hostName = "localhost")
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostName
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        return new RabbitMqEventBus(connection, channel);
    }

    public async Task PublishAsync<T>(T @event)
    {
        var eventName = typeof(T).Name;
        string exchange = "incident-exchange";

        await _channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Fanout   // pub/sub   
        );

        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        var basicProperties = new BasicProperties();

        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: "",
            basicProperties: basicProperties,
            mandatory: false,
            body: body
        );
    }
}
