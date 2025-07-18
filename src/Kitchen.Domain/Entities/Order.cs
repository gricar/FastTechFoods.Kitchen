using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public List<OrderItem> OrderItems { get; private set; } = new();
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    private Order() { }

    public Order(Guid OrderId, Guid customerId, List<OrderItem> orderItems, decimal totalPrice)
    {
        Id = OrderId;
        CustomerId = customerId;
        OrderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
        TotalPrice = totalPrice;
        Status = OrderStatus.Pending;
        LastModified = DateTime.UtcNow;
    }

    public void Accept()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Only pending orders can be accepted.");
        }
        Status = OrderStatus.Accepted;
        LastModified = DateTime.UtcNow;
        LastModifiedBy = "KitchenService";
    }

    public void Reject()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Only pending orders can be rejected.");
        }
        Status = OrderStatus.Rejected;
        LastModified = DateTime.UtcNow;
        LastModifiedBy = "KitchenService";
    }
}
