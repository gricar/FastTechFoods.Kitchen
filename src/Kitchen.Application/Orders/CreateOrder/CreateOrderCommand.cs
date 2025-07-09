using Kitchen.Application.Orders.DTOs;
using MediatR;

namespace Kitchen.Application.Orders.CreateOrder
{
    public sealed record CreateOrderCommand(OrderDto Order) : IRequest<Guid>;
}
