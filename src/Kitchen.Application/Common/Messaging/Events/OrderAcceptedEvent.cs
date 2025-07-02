using Kitchen.Domain.Entities;

namespace Kitchen.Application.Common.Messaging.Events;

public record OrderAcceptedEvent(OrderDto Order) : IntegrationEvent;
