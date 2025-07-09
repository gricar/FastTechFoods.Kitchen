using MediatR;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommand(Guid OrderId) : IRequest<AcceptOrderResponse>;
