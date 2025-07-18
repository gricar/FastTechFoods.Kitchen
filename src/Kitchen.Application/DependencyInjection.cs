using Kitchen.Application.Common.Behaviors;
using Kitchen.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kitchen.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddIntegrationEventHandlers(Assembly.GetExecutingAssembly());

        services.AddHealthChecks()
            //.AddMongoDb(configuration.GetConnectionString("MongoDb")!, name: "MongoDB Health Check")
            .AddRabbitMQ(configuration.GetConnectionString("RabbitMQ")!, name: "RabbitMQ Health Check");

        return services;
    }
}
