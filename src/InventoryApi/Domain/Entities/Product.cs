using InventoryApi.Domain.Enums;

namespace InventoryApi.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<StockMovement> Movements { get; set; } = new();

    public int Stock => Movements.Sum(m => m.Direction == MovementDirection.Entry ? m.Quantity : -m.Quantity);

    public void RegisterEntry(int quantity, string reason)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        Movements.Add(new StockMovement
        {
            ProductId = Id,
            Quantity = quantity,
            Direction = MovementDirection.Entry,
            Reason = reason
        });
    }

    public void RegisterExit(int quantity, string reason)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (Stock < quantity) throw new InvalidOperationException("No stock available");

        Movements.Add(new StockMovement
        {
            ProductId = Id,
            Quantity = quantity,
            Direction = MovementDirection.Exit,
            Reason = reason
        });
    }
}