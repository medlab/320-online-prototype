using System.Net.Http;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "dotnet", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet v1"));
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

//app.MapGet("/", () => "Hello World From DotNet!");
app.MapGet("/", async () => "dotnet response:\n '"+await new HttpClient().GetStringAsync("http://127.0.0.1:9083")+'\'');

app.Run();
