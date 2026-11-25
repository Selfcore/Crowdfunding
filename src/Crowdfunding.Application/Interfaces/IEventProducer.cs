namespace Crowdfunding.Application.Interfaces;

public interface IEventProducer
{
    Task PublishAsync<T>(string topic, string key, T message, CancellationToken token = default)
        where T : class;
}