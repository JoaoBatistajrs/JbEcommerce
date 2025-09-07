namespace SalesApi.Domain.Entities;

public class OrderItem
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal Total => Quantity * Price;

    protected OrderItem() { } 

    public OrderItem(int productId, int quantity, decimal price)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }
}