using SalesApi.Application.Commands;
using SalesApi.Application.Interfaces;

namespace SalesApi.Endpoints;

public static class SalesEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/orders")
            .WithTags("Orders");
            //.RequireAuthorization();

        group.MapPost("/", async (CreateOrderCommand command, IOrderService orderService, CancellationToken ct) =>
        {
            var order = await orderService.CreateOrderAsync(command, ct);
            return Results.Created($"/orders/{order.Id}", order);
        });

        group.MapGet("/{id:int}", async (int id, IOrderService orderService, CancellationToken ct) =>
        {
            var order = await orderService.GetByIdAsync(id, ct);
            return order is not null ? Results.Ok(order) : Results.NotFound();
        });
    }
}
