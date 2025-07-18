using HealthChecks.UI.Client;
using Kitchen.Api.Exceptions;
using Kitchen.Application.Common.Messaging.Events;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Application.Orders.EventHandlers.Integration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;

namespace Kitchen.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();

        eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>("order-created");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kitchen API");
                c.RoutePrefix = string.Empty; // Redireciona a url / para o Swagger
            });
        }

        app.UseExceptionHandler(options => { });

        app.UseMetricServer();

        app.UseHttpMetrics();

        app.MapControllers();

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
