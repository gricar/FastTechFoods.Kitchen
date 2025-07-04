using Kitchen.Application.Infrastructure.Data;
using Kitchen.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kitchen.Application.Orders.CreateOrder
{
    public sealed class CreateOrderCommandHandler(
        IKitchenMongoDbContext dbContext,
        ILogger<CreateOrderCommandHandler> logger)
        : IRequestHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order(
                command.Order.Id,
                command.Order.CustomerId,
                command.Order.OrderItems.Select(x => new OrderItem(x.OrderId, x.ProductId, x.Quantity, x.Price)).ToList(),
                command.Order.TotalPrice);

            logger.LogInformation("Order: {Order}", JsonSerializer.Serialize<Order>(order));

            await dbContext.Orders.InsertOneAsync(order, cancellationToken: cancellationToken);

            logger.LogInformation("Order {OrderId} saved to MongoDB.", order.Id);

            return order.Id;
        }
    }
}
