namespace InventoryApi.Application.ApiModels;

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public List<SaleItemModel> Items { get; set; } = new();
}

public class SaleItemModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderValidatedEvent
{
    public int OrderId { get; set; }
    public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;
}

public class OrderRejectedEvent
{
    public int OrderId { get; set; }
    public List<string> Errors { get; set; } = new();
    public DateTime RejectedAt { get; set; } = DateTime.UtcNow;
}