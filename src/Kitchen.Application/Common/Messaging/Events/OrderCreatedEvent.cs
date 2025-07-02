using Kitchen.Domain.Entities;

namespace Kitchen.Application.Common.Messaging.Events;

public record OrderCreatedEvent(OrderDto Order) : IntegrationEvent;
