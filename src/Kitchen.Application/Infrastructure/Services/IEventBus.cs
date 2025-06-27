namespace Kitchen.Application.Infrastructure.Services;

public interface IEventBus
{
    Task PublishAsync<T>(T message, string queueName);
}
