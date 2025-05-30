using Microsoft.EntityFrameworkCore;
using PhotoSi.UsersService.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqlLite"));
});

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    //dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
    //DatabaseSeeder.Seed(dbContext);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
