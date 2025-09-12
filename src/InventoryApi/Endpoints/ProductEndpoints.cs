using InventoryApi.Application.Interfaces;

namespace InventoryApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/products")
            .WithTags("Products");
        //.RequireAuthorization();

        group.MapPost("/", async (ProductDto dto, IInventoryService service, CancellationToken ct) =>
        {
            var product = await service.CreateProductAsync(dto.Name, dto.Description, dto.Price);
            return Results.Created($"/products/{product.Id}", product);
        });

        group.MapGet("/{id:int}", async (int id, IInventoryService service, CancellationToken ct) =>
        {
            var product = await service.GetProductByIdAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });

        group.MapGet("/", async (IInventoryService service, CancellationToken ct) =>
        {
            var products = await service.GetAllProductsAsync();
            return Results.Ok(products);
        });

        group.MapDelete("/{id:int}", async (int id, IInventoryService service, CancellationToken ct) =>
        {
            var deleted = await service.DeleteProductAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        group.MapGet("/{id:int}/stock", async (int id, IInventoryService service, CancellationToken ct) =>
        {
            var stock = await service.GetStockAsync(id);
            return Results.Ok(new { ProductId = id, Stock = stock });
        });

        group.MapGet("/{id:int}/movements", async (int id, IInventoryService service, CancellationToken ct) =>
        {
            var movements = await service.GetMovementsAsync(id);
            return Results.Ok(movements);
        });

        group.MapPost("/{id:int}/entry", async (MovementDto dto, int id, IInventoryService service, CancellationToken ct) =>
        {
            var result = await service.RegisterEntryAsync(id, dto.Quantity, dto.Reason);
            return result ? Results.Ok() : Results.BadRequest("Product Not Found or invalid data.");
        });

        group.MapPost("/{id:int}/exit", async (MovementDto dto, int id, IInventoryService service, CancellationToken ct) =>
        {
            var result = await service.RegisterExitAsync(id, dto.Quantity, dto.Reason);
            return result ? Results.Ok() : Results.BadRequest("Insufficient stock or product not found.");
        });
    }
}

public record ProductDto(string Name, string Description, decimal Price);
public record MovementDto(int Quantity, string Reason);
