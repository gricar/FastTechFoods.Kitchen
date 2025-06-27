using MediatR;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommand : IRequest<AcceptOrderResponse>
{
    public Guid OrderId { get; set; } // Internal Kitchen Order ID
}
