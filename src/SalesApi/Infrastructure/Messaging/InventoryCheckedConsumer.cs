using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SalesApi.Application.Events;
using SalesApi.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace SalesApi.Infrastructure.Messaging;

public class InventoryCheckedConsumer : BackgroundService
{
    private readonly ConnectionFactory _factory;
    private readonly IOrderRepository _orderRepository;
    private IConnection _connection;
    private IChannel _channel;

    public InventoryCheckedConsumer(
        IConfiguration configuration,
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = await _factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var exchange = "inventory.exchange";
        var queue = "sales.inventory.queue";

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true);
        await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
        await _channel.QueueBindAsync(queue, exchange, string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (ch, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var evt = JsonSerializer.Deserialize<InventoryCheckedEvent>(message);

                if (evt != null)
                {
                    var order = await _orderRepository.GetByIdAsync(evt.OrderId, stoppingToken);
                    if (order is not null)
                    {
                        if (evt.IsValid)
                            order.Confirm();
                        else
                            order.Cancel();

                        await _orderRepository.UpdateAsync(order);
                    }
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no consumo de InventoryChecked: {ex.Message}");
            }
        };

        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer);
    }
}
