using Kitchen.Application.Infrastructure.Services;
using Kitchen.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kitchen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var uri = configuration.GetConnectionString("RabbitMq") ?? "amqp://messagebroker:5672";
            var connectionName = configuration["MessageBroker:ConnectionName"] ?? "Kitchen.API";
            return new RabbitMQEventBus(uri, connectionName, logger);
        });

        return services;
    }
}
