namespace Kitchen.Application.Orders.RejectOrder;

public sealed record RejectedOrderResponse
{
    public Guid OrderId { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? RejectedAt { get; set; }
}
