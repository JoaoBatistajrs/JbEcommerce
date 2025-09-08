using Microsoft.EntityFrameworkCore;
using SalesApi.Endpoints;
using SalesApi.Infrastructure.Database;
using SalesApi.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraStrucuture(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<SalesContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error aplying migrations");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapOrderEndpoints();
app.Run();