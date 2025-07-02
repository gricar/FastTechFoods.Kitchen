using Kitchen.Application.Common.Messaging.Events;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Application.Orders.CreateOrder;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kitchen.Application.Orders.EventHandlers.Integration;

public sealed class OrderCreatedEventHandler(
    ISender sender, ILogger<OrderCreatedEventHandler> logger)
    : IIntegrationEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent @event)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", @event.EventType);
        logger.LogInformation("Order {OrderId} was created by Order MS.", @event.Order.Id);

        await sender.Send(new CreateOrderCommand(@event.Order));

        await Task.CompletedTask;
    }
}
