using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.UsersService;
using PhotoSi.UsersService.Database;
using PhotoSi.UsersService.Repositories;
using PhotoSi.UsersService.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqlLite"));
});

builder.Services.Configure<UsersServiceOptions>(
    builder.Configuration.GetSection("Services"));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


//builder.Services.AddHttpClient<AddressChecker>((serviceProvider, httpClient) =>
//{
//    var options = serviceProvider.GetRequiredService<IOptions<UsersServiceOptions>>();
//    httpClient.BaseAddress = new Uri("http://localhost:5001/");
//});
builder.Services.AddHttpClient<IAddressChecker, AddressChecker>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IAddressChecker, AddressChecker>();


builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    dbContext.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
