using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine("Hello, World!");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Console.WriteLine("Hello, World!");

var ttt=Environment.GetEnvironmentVariable("TTT");
var msg=ttt==""?"ttt does not exit":ttt;

Console.WriteLine(msg+"");

app.Run();
