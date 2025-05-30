using PhotoSi.ProductsService.Database;
using PhotoSi.Shared.Middleware;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.ProductsService.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddFeatureServices();
builder.Services.AddControllers();

var app = builder.Build();

app.ApplyMigrations();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
