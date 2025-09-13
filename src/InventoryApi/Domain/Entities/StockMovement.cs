using InventoryApi.Domain.Enums;

namespace InventoryApi.Domain.Entities;

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public MovementDirection Direction { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}