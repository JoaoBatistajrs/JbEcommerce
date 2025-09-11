using RabbitMQ.Client;
using SalesApi.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace SalesApi.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqEventPublisher(IConfiguration configuration)
    {
        var host = configuration["RabbitMQ:Host"];
        var user = configuration["RabbitMQ:Username"];
        var pass = configuration["RabbitMQ:Password"];

        _factory = new ConnectionFactory
        {
            HostName = host,
            UserName = user,
            Password = pass
        };
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var exchange = "order.exchange";
        var queue = "order.exchange.queue";

        _connection = await _factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Fanout,
            durable: true,
            cancellationToken: cancellationToken
        );

        var queueArgs = new Dictionary<string, object>
        {
            { "x-message-ttl", 3600000 }
        };

        await _channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs
        );

        await _channel.QueueBindAsync(
            queue: queue,
            exchange: exchange,
            routingKey: string.Empty
        );
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
    {
        if (_channel is null)
        {
            throw new InvalidOperationException("RabbitMQ connection and channel not initialized. Call InitializeAsync first.");
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

        var props = new BasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = DeliveryModes.Persistent;

        await _channel.BasicPublishAsync(
            exchange: "order.exchange",
            routingKey: string.Empty,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: cancellationToken
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null && _channel.IsOpen)
            await _channel.DisposeAsync();

        if (_connection is not null && _connection.IsOpen)
            await _connection.DisposeAsync();
    }
}