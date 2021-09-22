using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloApiController : ControllerBase
{
    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };

    // private readonly ILogger<WeatherForecastController> _logger;

    // public WeatherForecastController(ILogger<WeatherForecastController> logger)
    // {
    //     _logger = logger;
    // }

    // [HttpGet]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateTime.Now.AddDays(index),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }
    [HttpGet]
    [Route("[action]")]
    public string SayHello(string name)
    {
        return $"Hello {name}";
    }
    [HttpPost]
    [Route("[action]")]
    public string UploadFile(IFormFile file)
    {
        return $"Hello {file.Length}";
    }

}
