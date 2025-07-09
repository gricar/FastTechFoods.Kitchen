using Kitchen.Application.Orders.DTOs;

namespace Kitchen.Application.Common.Messaging.Events;

public record OrderCreatedEvent(OrderDto Order) : IntegrationEvent;
