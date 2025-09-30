namespace SalesApi.Infrastructure.Messaging;

public class RabbitMqHostedService : BackgroundService
{
    private readonly RabbitMqEventPublisher _publisher;

    public RabbitMqHostedService(RabbitMqEventPublisher publisher)
    {
        _publisher = publisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _publisher.InitializeAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _publisher.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
