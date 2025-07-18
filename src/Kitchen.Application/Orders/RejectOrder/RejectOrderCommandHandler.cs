using Kitchen.Application.Common.Messaging.Events;
using Kitchen.Application.Exceptions;
using Kitchen.Application.Infrastructure.Data;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace Kitchen.Application.Orders.RejectOrder;

public sealed record RejectOrderCommandHandler(
    IKitchenMongoDbContext dbContext,
    ILogger<RejectOrderCommandHandler> logger,
    IEventBus eventBus)
    : IRequestHandler<RejectOrderCommand, RejectedOrderResponse>
{
    public async Task<RejectedOrderResponse> Handle(RejectOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Find(x => x.Id == command.OrderId)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order with ID: {OrderId} not found.", command.OrderId);
            throw new OrderNotFoundException(command.OrderId);
        }

        order.Reject();

        await dbContext.Orders.ReplaceOneAsync(
            x => x.Id == order.Id,
            order,
            cancellationToken: cancellationToken);

        logger.LogInformation("Order successfully rejected - Order: {Order}", JsonSerializer.Serialize<Order>(order));

        var eventMsg = new OrderRejectedEvent(order.Id);

        await eventBus.PublishAsync(eventMsg, "order-rejected");

        return new RejectedOrderResponse
        {
            Message = "Order successfully rejected.",
            OrderId = order.Id,
            RejectedAt = order.LastModified.ToLocalTime(),
            IsSuccess = true
        };
    }
}
