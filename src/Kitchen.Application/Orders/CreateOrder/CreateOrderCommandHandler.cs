using Kitchen.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kitchen.Application.Orders.CreateOrder
{
    public sealed class CreateOrderCommandHandler(
        ILogger<CreateOrderCommandHandler> logger)
        : IRequestHandler<CreateOrderCommand, Guid>
    {
        public Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling CreateOrderCommand for Order: {Order}", JsonSerializer.Serialize<OrderDto>(command.Order));

            var order = new Order(
                command.Order.Id,
                command.Order.CustomerId,
                command.Order.OrderItems.Select(x => new OrderItem(x.OrderId, x.ProductId, x.Quantity, x.Price)).ToList(),
                command.Order.TotalPrice);
            logger.LogInformation("Order: {Order}", JsonSerializer.Serialize<Order>(order));

            //save order to database
            return Task.FromResult(order.Id);
        }
    }
}
