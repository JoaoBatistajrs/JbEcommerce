namespace SalesApi.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string exchange, string routingKey = "", CancellationToken cancellationToken = default);
}
