using MediatR;

namespace Kitchen.Application.Orders.RejectOrder;

public sealed record RejectOrderCommand(Guid OrderId) : IRequest<RejectedOrderResponse>;
