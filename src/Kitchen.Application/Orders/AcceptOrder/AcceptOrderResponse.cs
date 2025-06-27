namespace Kitchen.Application.Orders.AcceptOrder;

public sealed record AcceptOrderResponse
{
    public Guid OrderId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? AcceptedAt { get; set; }
}
