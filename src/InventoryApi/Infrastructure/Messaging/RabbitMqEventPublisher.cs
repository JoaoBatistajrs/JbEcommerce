using InventoryApi.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InventoryApi.Infrastructure.Messaging;

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
        _connection = await _factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
    }

    public async Task PublishAsync<T>(
        T @event,
        string exchange,
        string routingKey = "",
        CancellationToken cancellationToken = default)
    {
        if (_channel is null)
        {
            throw new InvalidOperationException("RabbitMQ connection and channel not initialized. Call InitializeAsync first.");
        }

        await _channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Fanout,
            durable: true,
            cancellationToken: cancellationToken
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
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
