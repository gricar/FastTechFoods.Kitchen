using Kitchen.Domain.Entities;
using MediatR;

namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderCommand(OrderDto Order) : IRequest<AcceptOrderResponse>;
