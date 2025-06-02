using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("http://localhost:6781/swagger/v1/swagger.json", "AddressBook Service");
        opt.SwaggerEndpoint("http://localhost:6782/swagger/v1/swagger.json", "Users Service");
        opt.SwaggerEndpoint("http://localhost:6783/swagger/v1/swagger.json", "Products Service");
        opt.SwaggerEndpoint("http://localhost:6784/swagger/v1/swagger.json", "Orders Service");
    });
}

app.MapReverseProxy();

app.Run();
