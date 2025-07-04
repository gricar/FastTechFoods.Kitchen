using Kitchen.Application.Common.Messaging.Events;
using Kitchen.Application.Infrastructure.Data;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommandHandler(
    IKitchenMongoDbContext dbContext,
    ILogger<AcceptOrderCommandHandler> logger,
    IEventBus eventBus)
    : IRequestHandler<AcceptOrderCommand, AcceptOrderResponse>
{
    public async Task<AcceptOrderResponse> Handle(AcceptOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Find(x => x.Id == command.Order.Id).FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order with ID: {OrderId} not found.", command.Order.Id);
            return new AcceptOrderResponse
            {
                OrderId = command.Order.Id,
                Success = false,
                Message = $"Order with ID {command.Order.Id} not found."
            };
        }

        order.Accept();

        await dbContext.Orders.ReplaceOneAsync(
            x => x.Id == order.Id,
            order,
            cancellationToken: cancellationToken);

        logger.LogInformation("Order successfully accepted - Order: {Order}", JsonSerializer.Serialize<Order>(order));

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
