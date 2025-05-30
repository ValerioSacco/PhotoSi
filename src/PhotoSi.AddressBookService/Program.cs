using Microsoft.EntityFrameworkCore;
using PhotoSi.AddressBookService.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AddressBookDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqlLite"));
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AddressBookDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
