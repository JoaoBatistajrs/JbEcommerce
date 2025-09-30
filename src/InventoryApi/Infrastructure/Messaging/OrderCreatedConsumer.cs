using InventoryApi.Application.ApiModels;
using InventoryApi.Application.Interfaces;
using InventoryApi.Infrastructure.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InventoryApi.Infrastructure.Messaging;

public class OrderCreatedConsumer : BackgroundService
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IChannel _channel;
    private readonly IProductService _productService;
    private readonly IEventPublisher _publisher;
    private readonly RabbitMqOptions _options;

    public OrderCreatedConsumer(
        IConfiguration configuration,
        IProductService productService,
        IEventPublisher publisher,
        RabbitMqOptions options)
    {
        _productService = productService;
        _publisher = publisher;
        _options = options;

        _factory = new ConnectionFactory
        {
            HostName = _options.Host,
            UserName = _options.Username,
            Password = _options.Password
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = await _factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var exchange = "order.exchange";
        var queue = "inventory.order.queue";

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true);

        await _channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        await _channel.QueueBindAsync(queue, exchange, string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (ch, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var orderCreated = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                if (orderCreated == null)
                {
                    Console.WriteLine("[Inventory] Mensagem inválida recebida.");
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    return;
                }

                Console.WriteLine($"[Inventory] Pedido recebido: {orderCreated.OrderId}");

                var validation = await _productService.ValidateSaleAsync(
                    orderCreated.OrderId,
                    orderCreated.Items.Select(i => new SaleItemModel
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    }).ToList()
                );

                if (validation.IsValid)
                {
                    var evt = new OrderValidatedEvent { OrderId = orderCreated.OrderId };
                    await _publisher.PublishAsync(evt, "inventory.validated.exchange");

                    Console.WriteLine($"[Inventory] Order {orderCreated.OrderId} aproved");
                }
                else
                {
                    var evt = new OrderRejectedEvent
                    {
                        OrderId = orderCreated.OrderId,
                        Errors = validation.Errors
                    };
                    await _publisher.PublishAsync(evt, "inventory.rejected.exchange");

                    Console.WriteLine($"[Inventory] Order {orderCreated.OrderId} rejected");
                    foreach (var error in validation.Errors)
                    {
                        Console.WriteLine($"   - {error}");
                    }
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Inventory] Erro no consumo: {ex.Message}");
            }
        };

        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        return base.StopAsync(cancellationToken);
    }
}
