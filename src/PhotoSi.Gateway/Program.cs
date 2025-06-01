var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    //app.UseSwagger();
//    //app.UseSwaggerUI(c =>
//    //{
//    //    c.SwaggerEndpoint("users-service/swagger/v1/swagger.json", "Users Service");
//    //    c.SwaggerEndpoint("products-service/swagger/v1/swagger.json", "Products Service");
//    //    c.RoutePrefix = "swagger";
//    //});
//}

app.MapReverseProxy();


app.Run();
