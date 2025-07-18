namespace Kitchen.Application.Common.Messaging.Events;

public record OrderRejectedEvent(Guid OrderId) : IntegrationEvent;
