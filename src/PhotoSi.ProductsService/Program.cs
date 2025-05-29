using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Features.Shared;
using PhotoSi.ProductsService.Middleware;
using PhotoSi.ProductsService.Repositories;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductsDbContext>(opt =>
{
    opt.UseInMemoryDatabase("ProductServiceDb")
    .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    //config.AddOpenBehavior(typeof(CommandTransactionBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
});
//builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionBehavior<,>));
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));


builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUnitOfWork, ProductsUnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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
