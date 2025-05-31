using MassTransit;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Features;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddFeatureServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(opt =>
{
    opt.SetKebabCaseEndpointNameFormatter();
    opt.AddConsumer<ProductCreatedEventConsumer>();

    opt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["RabbitMQ:Host"]!), h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]!);
            h.Password(builder.Configuration["RabbitMQ:Password"]!);
        });

        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();

app.Run();
