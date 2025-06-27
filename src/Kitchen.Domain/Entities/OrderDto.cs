using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Entities;

public sealed record OrderDto(
    Guid OrderId,
    Guid CustomerId,
    List<OrderItemDto> OrderItems,
    decimal TotalPrice,
    OrderStatus Status);


public sealed record OrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal Price);
