using MassTransit;
using Messaging;
using Microsoft.EntityFrameworkCore;
using ProductWebApi;
using StockWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/* Database Context Dependency Injection */
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD");

var connectionString = $"server={dbHost};port=3306;database={dbName};user=root;password={dbPassword}";
builder.Services.AddDbContext<ProductDbContext>(o => o.UseMySQL(connectionString));

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<OrderCardNumberValidateConsumer>();

    cfg.AddBus(provider => RabbitMqBus.ConfigureBus(provider, (cfg, host) =>
    {
        cfg.ReceiveEndpoint(BusConstants.OrderQueue, ep =>
        {
            ep.ConfigureConsumer<OrderCardNumberValidateConsumer>(provider);
        });
    }));
});

builder.Services.AddMassTransitHostedService();

/* ===================================== */

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
