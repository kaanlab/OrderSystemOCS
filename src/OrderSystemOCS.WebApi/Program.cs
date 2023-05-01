using OrderSystemOCS.WebApi.Exceptions;
using OrderSystemOCS.WebApi.Models;
using OrderSystemOCS.WebApi.Services;
using OrderSystemOCS.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDatabaseLayer(connectionString);

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  
}

// global error handler
app.UseMiddleware<ExceptionMiddleware>();

app.MapGet("/orders", async (IOrderService service) => await service.GetAll())
   .Produces<IReadOnlyList<OrderResponse>>(StatusCodes.Status200OK)
   .WithName("GetOrder")   
   .WithOpenApi();

app.MapGet("/orders/{id}", async (Guid id, IOrderService service) => {
        var order = await service.GetById(id);
        return order is null ? Results.NotFound() : Results.Ok(order);
    })
   .Produces<OrderResponse>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound)
   .WithName("GetOrderById")
   .WithOpenApi();

app.MapPost("/orders", async (NewOrderRequest request, IOrderService service) =>
    {
        var order = await service.Create(request);
        return Results.Ok(order);
    })
   .Produces<OrderResponse>(StatusCodes.Status200OK)
   .WithName("CreateOrder")
   .WithOpenApi(); 

app.MapPut("/orders/{id}", async (Guid id, UpdateOrderRequest request, IOrderService service) =>
    {
        var order = await service.Update(id, request);
        return order is null ? Results.NotFound() : Results.Ok(order);
    })
    .Produces<OrderResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("UpdateOrder")
    .WithOpenApi(); 

app.MapDelete("/orders/{id}", async (Guid id, IOrderService service) =>
    {
        var result = await service.Delete(id);
        return result ? Results.Ok() : Results.NotFound();
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("DeleteOrder")
    .WithOpenApi(); 

app.Run();
