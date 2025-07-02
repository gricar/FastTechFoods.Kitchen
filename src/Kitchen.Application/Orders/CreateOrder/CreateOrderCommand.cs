using Kitchen.Domain.Entities;
using MediatR;

namespace Kitchen.Application.Orders.CreateOrder
{
    public sealed record CreateOrderCommand(OrderDto Order) : IRequest<Guid>;
}
