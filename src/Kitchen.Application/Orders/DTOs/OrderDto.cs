using Kitchen.Domain.Enums;

namespace Kitchen.Application.Orders.DTOs;

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    List<OrderItemDto> OrderItems,
    decimal TotalPrice,
    OrderStatus Status);

public sealed record OrderItemDto(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Price);