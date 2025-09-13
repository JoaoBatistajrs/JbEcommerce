using InventoryApi.Domain.Enums;

namespace InventoryApi.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CurrentStock { get; private set; } = 0;


    public List<StockMovement> Movements { get; set; } = new();

    public void ApplyMovement(int quantity, MovementDirection direction)
    {
        if (direction == MovementDirection.Entry)
            CurrentStock += quantity;
        else
        {
            if (CurrentStock < quantity)
                throw new InvalidOperationException("No stock available");
            CurrentStock -= quantity;
        }

        Movements.Add(new StockMovement
        {
            Quantity = quantity,
            Direction = direction,
            Date = DateTime.UtcNow
        });
    }
}