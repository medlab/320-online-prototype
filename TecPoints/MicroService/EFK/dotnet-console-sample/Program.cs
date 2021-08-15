// See https://aka.ms/new-console-template for more information
// ref: https://github.com/serilog/serilog/wiki/Configuration-Basics
// ref: https://github.com/serilog/serilog/wiki/Enrichment

using System;
using System.Threading.Tasks;
using Serilog;

Log.Logger = new LoggerConfiguration()
 .Enrich.WithThreadId()
 .WriteTo.Console()
    .CreateLogger();

Log.Information("--begin--");

int counter=1;
do{
    Log.Information("counter: {counter}", counter);
    await Task.Delay(TimeSpan.FromSeconds(1));
    counter++;

}while(true);


Log.Information("--end--");

Log.CloseAndFlush();