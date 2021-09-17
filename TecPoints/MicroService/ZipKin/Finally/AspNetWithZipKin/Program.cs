using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AspNetWithZipKin", Version = "v1" });
});

builder.Services.AddOpenTelemetryTracing(config=>
    config.AddZipkinExporter(object=>
    {

    })
    .AddZipkinHttpExporter(object=>
    {}));   


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetWithZipKin v1"));
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
