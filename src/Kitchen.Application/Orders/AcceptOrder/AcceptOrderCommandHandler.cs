using Kitchen.Application.Common.Messaging.Events;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommandHandler(
    ILogger<AcceptOrderCommandHandler> logger,
    IEventBus eventBus)
    : IRequestHandler<AcceptOrderCommand, AcceptOrderResponse>
{
    public async Task<AcceptOrderResponse> Handle(AcceptOrderCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Kitchen.API handling {command} for OrderId: {OrderId}", nameof(AcceptOrderCommand), command.Order.OrderId);

        var orderItems = command.Order.OrderItems.Adapt<List<OrderItem>>();

        var order = new Order(command.Order.OrderId, command.Order.CustomerId, orderItems, command.Order.TotalPrice);

        order.Accept();

        logger.LogInformation("Order accepted - Order: {Order}", JsonSerializer.Serialize<Order>(order));

        var eventMsg = new OrderAcceptedEvent(new OrderDto(order.Id, order.CustomerId, command.Order.OrderItems, order.TotalPrice, order.Status));

        await eventBus.PublishAsync(eventMsg, "order-accepted");

        return new AcceptOrderResponse
        {
            Message = "Order accepted successfully.",
            OrderId = order.Id,
            AcceptedAt = order.LastModified.ToLocalTime(),
            Success = true
        };
    }
}
