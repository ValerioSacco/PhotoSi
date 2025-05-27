using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductsDbContext>(opt =>
{
    opt.UseInMemoryDatabase("ProductServiceDb");
});

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    DatabaseSeeder.Seed(dbContext);
}


app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
