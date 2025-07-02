using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Entities;

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
