namespace Kitchen.Domain.Entities;

//public class OrderItem
//{
//    public Guid OrderId { get; private set; }
//    public Guid ProductId { get; private set; }
//    public int Quantity { get; private set; }
//    public decimal Price { get; private set; }
//}

public sealed record OrderItem(Guid OrderId, Guid ProductId, int Quantity, decimal Price);
