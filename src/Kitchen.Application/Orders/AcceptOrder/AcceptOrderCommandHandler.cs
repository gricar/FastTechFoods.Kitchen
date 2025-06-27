using Kitchen.Application.Common.Messaging.Events;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommandHandler(
    ILogger<AcceptOrderCommandHandler> logger,
    IEventBus eventBus)
    : IRequestHandler<AcceptOrderCommand, AcceptOrderResponse>
{
    public async Task<AcceptOrderResponse> Handle(AcceptOrderCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Kitchen.API handling {command} for OrderId: {OrderId}", nameof(AcceptOrderCommand), command.order.OrderId);

        var orderItems = command.order.OrderItems
            .Select(item => new OrderItem(command.order.OrderId, item.ProductId, item.Quantity, item.Price))
            .ToList();

        var order = new Order(command.order.OrderId, command.order.CustomerId, orderItems, command.order.TotalPrice);

        order.Accept();

        var eventMsg = new OrderAcceptedEvent(new OrderDto(order.Id, order.CustomerId, command.order.OrderItems, order.TotalPrice, order.Status));

        await eventBus.PublishAsync(eventMsg, "order-accepted");

        logger.LogInformation("OrderAcceptedEvent published for Order: {Order}", order);

        return new AcceptOrderResponse
        {
            Message = "Order accepted successfully.",
            OrderId = order.Id,
            AcceptedAt = order.LastModified.ToLocalTime(),
            Success = true
        };
    }
}
