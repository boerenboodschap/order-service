// using Microsoft.EntityFrameworkCore;
using GettingStarted;
using MassTransit;
using order_service.Models;
using order_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.Configure<OrderDatabaseSettings>(
    builder.Configuration.GetSection("orderDatabase"));

builder.Services.AddSingleton<OrdersService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(ctx);
    });
});
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapGet("/", () => "Hello World!");

// app.Run();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

