using InventoryApi.Application.ApiModels;
using InventoryApi.Application.Interfaces;

namespace InventoryApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/products")
            .WithTags("Products");
        //.RequireAuthorization();

        group.MapPost("/", async (ProductModelRequest productCreateRequest, IProductService service, CancellationToken ct) =>
        {
            var product = await service.CreateProductAsync(productCreateRequest);
            return Results.Created($"/products/{product.Id}", product);
        });

        group.MapGet("/", async (IProductService service, CancellationToken ct) =>
        {
            var products = await service.GetAll();
            return Results.Ok(products);
        });

        group.MapGet("/{id:int}", async (int id, IProductService service, CancellationToken ct) =>
        {
            var product = await service.GetById(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });
    }
}