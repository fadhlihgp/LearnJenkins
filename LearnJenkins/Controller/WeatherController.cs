using Microsoft.AspNetCore.Mvc;

namespace LearnJenkins.Controller;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private string[] _summaries;

    public WeatherController()
    {
        _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }

    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    [HttpGet("hello-world")]
    public IActionResult HelloWorld()
    {
        return Ok(new
        {
            Message = "Hallo world!"
        });
    }
    
    [HttpGet("list")]
    public IActionResult List()
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray();
        return Ok(forecast);
    }
    
    [HttpGet("detail/{is}")]
    public IActionResult GetById([FromRoute] int id)
    {
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray()[id];
        return Ok(forecast);
    }
}