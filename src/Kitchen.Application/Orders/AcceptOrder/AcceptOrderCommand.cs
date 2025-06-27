using Kitchen.Domain.Entities;
using MediatR;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommand(OrderDto order) : IRequest<AcceptOrderResponse>;
//{
//    public Guid OrderId { get; set; }
//    public Guid CustomerId { get; set; }
//    public List<OrderItemDto> OrderItems { get; set; } = new();
//    public decimal TotalPrice { get; set; }
//}
