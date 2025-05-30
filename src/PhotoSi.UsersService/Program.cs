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

var app = builder.Build();

app.ApplyMigrations();
app.UseAuthorization();
app.MapControllers();

app.Run();
