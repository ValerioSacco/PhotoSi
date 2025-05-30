using PhotoSi.Shared.Middleware;
using PhotoSi.UsersService;
using PhotoSi.UsersService.Database;
using PhotoSi.UsersService.Features;
using PhotoSi.UsersService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UsersServiceOptions>(
    builder.Configuration.GetSection("Services"));

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddFeatureServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
