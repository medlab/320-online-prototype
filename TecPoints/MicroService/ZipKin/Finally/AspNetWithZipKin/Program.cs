using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AspNetWithZipKin", Version = "v1" });
});

builder.Services.AddOpenTelemetryTracing(config =>
{
    config.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(builder.Environment.ApplicationName));
    
    config.AddAspNetCoreInstrumentation();
    config.AddHttpClientInstrumentation();
    
    config.AddZipkinExporter(o =>
    {
        o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
        //o.ServiceName 
    });
    config.AddConsoleExporter();
});


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
