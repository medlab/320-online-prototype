// See https://aka.ms/new-console-template for more information
// ref: https://github.com/serilog/serilog/wiki/Configuration-Basics
// ref: https://github.com/serilog/serilog/wiki/Enrichment

using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Elasticsearch;

Log.Logger = new LoggerConfiguration()
 .Enrich.WithThreadId()
  // ref: https://github.com/serilog/serilog-sinks-console
  // ref: https://github.com/serilog/serilog-sinks-elasticsearch#elasticsearch-formatters
  // ref: https://www.cnblogs.com/jesen1315/p/11398597.html
 .WriteTo.Console(new ElasticsearchJsonFormatter()) 
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