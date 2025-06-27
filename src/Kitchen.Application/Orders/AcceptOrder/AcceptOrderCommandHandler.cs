using Kitchen.Application.Infrastructure.Services;
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
        logger.LogInformation("Kitchen.API handling {command} for OrderId: {OrderId}", nameof(AcceptOrderCommand), command.OrderId);

        var eventMsg = new OrderAcceptedEvent(command.OrderId, "Accepted", DateTime.Now);

        await eventBus.PublishAsync(eventMsg, "order-accepted");

        logger.LogInformation("OrderAcceptedEvent published for OrderId: {OrderId}", command.OrderId);

        return new AcceptOrderResponse
        {
            Message = "Order accepted successfully.",
            OrderId = command.OrderId,
            AcceptedAt = DateTime.Now,
            Success = true
        };
    }
}

public sealed record OrderAcceptedEvent(Guid OrderId, string Status, DateTime AcceptedAt);
