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
        opt.SwaggerEndpoint("http://localhost:5001/swagger/v1/swagger.json", "AddressBook Service");
        opt.SwaggerEndpoint("http://localhost:5002/swagger/v1/swagger.json", "Users Service");
        opt.SwaggerEndpoint("http://localhost:5003/swagger/v1/swagger.json", "Products Service");
        opt.SwaggerEndpoint("http://localhost:5004/swagger/v1/swagger.json", "Orders Service");
    });
}

app.MapReverseProxy();

app.Run();
